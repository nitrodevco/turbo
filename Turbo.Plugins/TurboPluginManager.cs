using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Turbo.Plugins
{
    public class TurboPluginManager : IPluginManager
    {
        private readonly HashSet<ITurboPlugin> _plugins = new HashSet<ITurboPlugin>();
        private readonly HashSet<MethodInfo> _methods = new HashSet<MethodInfo>();

        private readonly ILogger<TurboPluginManager> _logger;
        private readonly IServiceProvider _serviceProvider;

        public TurboPluginManager(ILogger<TurboPluginManager> logger, IServiceProvider serviceProvider) : base()
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public void LoadPlugins()
        {
            _logger.LogInformation("{Context} -> Loading plugins...", nameof(TurboPluginManager));

            if (!Directory.Exists("plugins"))
            {
                Directory.CreateDirectory("plugins");
            }

            var plugins = Directory.GetFiles("plugins", "*.dll");

            foreach (var plugin in plugins)
            {
                // Load assembly
                var assembly = Assembly.LoadFile(Path.Combine(Directory.GetCurrentDirectory(), plugin));

                // Get a list of all types in assembly that implement ITurboPlugin. 
                // Exclude interfaces, abstract and generic types.
                var pluginTypes = assembly.GetTypes()
                    .Where(t => typeof(ITurboPlugin).IsAssignableFrom(t))
                    .Where(t => t.IsClass && !t.IsAbstract && t.IsPublic && !t.IsGenericType)
                    .ToList();

                if (pluginTypes.Any())
                {
                    // Create instances
                    foreach (var pluginType in pluginTypes)
                    {
                        CreatePluginInstance(pluginType);
                    }
                }
                else
                {
                    _logger.LogError("{Context} -> {Plugin} can't be loaded because it doesn't implement {PluginInterface}!", nameof(TurboPluginManager), plugin, nameof(ITurboPlugin));
                }
            }

            _logger.LogInformation("{Context} -> {AmountOfPlugins} plugin(s) loaded!", nameof(TurboPluginManager), _plugins.Count);
        }

        private void CreatePluginInstance(Type pluginType)
        {
            var constructors = pluginType.GetConstructors();
            var firstConstrutor = constructors.FirstOrDefault(); // Assume we will have only one constructor
            var parameters = new List<object>();

            // Get plugin constructor params
            foreach (var param in firstConstrutor.GetParameters())
            {
                // Get instance of the param class
                var service = _serviceProvider.GetService(param.ParameterType);
                parameters.Add(service);
            }

            ITurboPlugin pluginInstance = null;

            // Create instance using DI container
            if (parameters.Count > 0)
            {
                pluginInstance = Activator.CreateInstance(pluginType, parameters.ToArray()) as ITurboPlugin;
                _plugins.Add(pluginInstance);
            }
            // Create instance without DI container
            else
            {
                pluginInstance = Activator.CreateInstance(pluginType) as ITurboPlugin;
                _plugins.Add(pluginInstance);
            }

            if (pluginInstance != null)
            {
                _logger.LogInformation("{Context} -> Loaded {PluginName} by {PluginAuthor}", nameof(TurboPluginManager), pluginInstance.PluginName, pluginInstance.PluginAuthor);
            }
        }
    }
}
