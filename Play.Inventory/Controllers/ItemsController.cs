using Microsoft.AspNetCore.Mvc;
using Play.Common.Repositories;
using Play.Inventory.Clients;
using Play.Inventory.DTO;
using Play.Inventory.Entities;
using Play.Inventory.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Inventory.Controllers
{
    [ApiController]
    [Route("api/items")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<InventoryItem> _itemsRepository;
        private readonly CatalogClient _catalogClient;

        public ItemsController(IRepository<InventoryItem> repository, CatalogClient catalogClient)
        {
            _itemsRepository = repository;
            _catalogClient = catalogClient;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDTO>>> GetAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest();
            }

            var catalogItemList = await _catalogClient.GetCatalogItemsAsync();
            var inventoryItemList = await _itemsRepository.GetAllAsync(item => item.UserId == userId);

            var inventoryItemsDto = inventoryItemList.Select(inventoryItem =>
            {
                var catalogItem = catalogItemList.Single(catalogItem => catalogItem.Id == inventoryItem.CatalogItemId);
                return inventoryItem.AsDTO(catalogItem.Name, catalogItem.Description);
            });

            return Ok(inventoryItemsDto);
        }
        [HttpPost]
        public async Task<ActionResult> PostAsync(GrantItemContract grantItemsContract)
        {
            var inventoryItem = await _itemsRepository.GetAsync(item => item.UserId == grantItemsContract.UserId
                && item.CatalogItemId == grantItemsContract.CatalogItemId);

            if (inventoryItem == null)
            {
                inventoryItem = new InventoryItem
                {
                    CatalogItemId = grantItemsContract.CatalogItemId,
                    UserId = grantItemsContract.UserId,
                    Quantity = grantItemsContract.Quantity,
                    AcquiredDate = DateTimeOffset.UtcNow
                };

                await _itemsRepository.CreateAsync(inventoryItem);
            }
            else
            {
                inventoryItem.Quantity += grantItemsContract.Quantity;
                await _itemsRepository.UpdateAsync(inventoryItem);
            }

            return Ok();
        }
    }
}
