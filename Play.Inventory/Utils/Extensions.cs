using Play.Inventory.DTO;
using Play.Inventory.Entities;

namespace Play.Inventory.Utils
{
    public static class Extensions
    {
        public static InventoryItemDTO AsDTO(this InventoryItem item, string name, string description)
        {
            return new InventoryItemDTO(item.CatalogItemId, item.Quantity, item.AcquiredDate, name, description);
        }
    }
}
