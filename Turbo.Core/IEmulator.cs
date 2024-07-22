using Microsoft.Extensions.Hosting;

namespace Turbo.Core;

public interface IEmulator : IHostedService
{
    public string GetVersion();
}