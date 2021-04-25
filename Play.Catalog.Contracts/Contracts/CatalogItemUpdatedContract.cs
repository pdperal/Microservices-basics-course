using System;

namespace Play.Catalog.Contracts.Contracts
{
    public record CatalogItemUpdatedContract(Guid ItemId, string Name, string Description);
}
