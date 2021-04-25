using System;

namespace Play.Catalog.Contracts.Contracts
{
    public record CatalogItemCreatedContract(Guid ItemId, string Name, string Description);
}
