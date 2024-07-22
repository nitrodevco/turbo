using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Catalog;

namespace Turbo.Catalog;

public class CatalogRoot(ILogger<ICatalogPage> _logger) : CatalogPage(_logger, null)
{
    public override int Id => -1;

    public override int ParentId => -1;
    public override int Icon => 0;
    public override string Name => "root";
    public override string Localization => "";

    public override bool Visible => true;

    public override void SetParent(ICatalogPage catalogPage)
    {
    }

    public override void AddChild(ICatalogPage catalogPage)
    {
        if (catalogPage == null || Children.ContainsKey(catalogPage.Id)) return;

        Children.Add(catalogPage.Id, catalogPage);

        catalogPage.SetParent(this);
    }

    public override void AddOffer(ICatalogOffer catalogItem)
    {
    }
}