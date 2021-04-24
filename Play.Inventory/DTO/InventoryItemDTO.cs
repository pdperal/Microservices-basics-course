using System;

namespace Play.Inventory.DTO
{
    public record InventoryItemDTO(Guid CatalogItemId, int Quantity, DateTimeOffset AcquiredDate);
}
