using System;

namespace Play.Inventory.DTO
{
    public record GrantItemContract(Guid UserId, Guid CatalogItemId, int Quantity);
}
