using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection; // added
using Turbo.Core.Configuration;
using Turbo.Core.Plugins;

namespace Turbo.Plugins;

public class TurboPluginManager(
    ILogger<TurboPluginManager> _logger,
    IServiceProvider _serviceProvider,
    IEmulatorConfig _emulatorConfig) : IPluginManager
{
    private readonly HashSet<MethodInfo> _methods = new();
    private readonly HashSet<ITurboPlugin> _plugins = new();

    public void LoadPlugins()
    {
        _logger.LogInformation("{Context} -> Loading plugins...", nameof(TurboPluginManager));

        if (!Directory.Exists("plugins")) Directory.CreateDirectory("plugins");

        var plugins = Directory.GetFiles("plugins", "*.dll");

        var pluginOrder = _emulatorConfig.PluginOrder?.ToArray();
        if (pluginOrder is not null && pluginOrder.Length > 0)
        {
            // Order by index in configured order list; unknown items get large index to appear last
            plugins = plugins
                .OrderBy(p =>
                {
                    var normalized = p.Replace('/', '\\');
                    return Array.IndexOf(pluginOrder, normalized);
                })
                .ToArray();
        }

        foreach (var plugin in plugins)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), plugin);
                var assembly = Assembly.LoadFrom(path);
                if (assembly is null)
                {
                    _logger.LogWarning("{Context} -> Failed to load assembly for {Plugin}", nameof(TurboPluginManager), plugin);
                    continue;
                }

                var pluginTypes = assembly.GetTypes()
                    .Where(t => typeof(ITurboPlugin).IsAssignableFrom(t))
                    .Where(t => t.IsClass && !t.IsAbstract && t.IsPublic && !t.IsGenericType)
                    .ToList();

                if (!pluginTypes.Any())
                {
                    _logger.LogError(
                        "{Context} -> {Plugin} can't be loaded because it doesn't implement {PluginInterface}!",
                        nameof(TurboPluginManager), plugin, nameof(ITurboPlugin));
                    continue;
                }

                foreach (var pluginType in pluginTypes)
                {
                    CreatePluginInstance(pluginType, plugin);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Context} -> {Plugin} not loaded", nameof(TurboPluginManager), plugin);
            }
        }

        _logger.LogInformation("{Context} -> {AmountOfPlugins} plugin(s) loaded!", nameof(TurboPluginManager),
            _plugins.Count);
    }

    private void CreatePluginInstance(Type pluginType, string sourcePath)
    {
        try
        {
            // Check constructor dependencies explicitly for clearer logs
            var ctor = pluginType.GetConstructors().OrderBy(c => c.GetParameters().Length).FirstOrDefault();
            if (ctor is null)
            {
                _logger.LogWarning("{Context} -> {PluginType} has no public constructor", nameof(TurboPluginManager), pluginType.FullName);
                return;
            }

            var missing = ctor.GetParameters()
                .Where(p => _serviceProvider.GetService(p.ParameterType) is null)
                .Select(p => p.ParameterType.FullName)
                .ToList();

            if (missing.Count > 0)
            {
                _logger.LogError("{Context} -> Cannot create {PluginType} from {Source}: missing DI services: {Missing}",
                    nameof(TurboPluginManager), pluginType.FullName, sourcePath, string.Join(", ", missing));
                return;
            }

            // Let ActivatorUtilities resolve dependencies (will pull from DI + additional args if provided)
            var pluginInstance = (ITurboPlugin)ActivatorUtilities.CreateInstance(_serviceProvider, pluginType);

            if (pluginInstance is null)
            {
                _logger.LogError("{Context} -> ActivatorUtilities returned null for {PluginType}", nameof(TurboPluginManager), pluginType.FullName);
                return;
            }

            if (_plugins.Add(pluginInstance))
            {
                _logger.LogInformation("{Context} -> Loaded {PluginName} by {PluginAuthor}", nameof(TurboPluginManager),
                    pluginInstance.PluginName, pluginInstance.PluginAuthor);
            }
        }
        catch (TargetInvocationException tie) when (tie.InnerException is not null)
        {
            _logger.LogError(tie.InnerException, "{Context} -> Exception constructing plugin {PluginType} (inner)", nameof(TurboPluginManager), pluginType.FullName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Context} -> Failed to construct plugin {PluginType}", nameof(TurboPluginManager), pluginType.FullName);
        }
    }
}