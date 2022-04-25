using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Catalog;

namespace Turbo.Catalog
{
    public class CatalogManager : ICatalogManager
    {
        private readonly ILogger<CatalogManager> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public CatalogManager(
            ILogger<CatalogManager> logger,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = scopeFactory;
        }

        public async ValueTask InitAsync()
        {

        }

        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }
    }
}
