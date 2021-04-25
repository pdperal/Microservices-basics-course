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
        private readonly IRepository<InventoryItem> _inventoryItemsRepository;
        private readonly IRepository<CatalogItem> _catalogItemsRepository;

        public ItemsController(IRepository<InventoryItem> repository, IRepository<CatalogItem> catalogItemsRepository)
        {
            _inventoryItemsRepository = repository;
            _catalogItemsRepository = catalogItemsRepository;
        }

        [HttpGet("GetInventoryByIdAsync")]
        public async Task<ActionResult<IEnumerable<InventoryItemDTO>>> GetInventoryByIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest();
            }

            var inventoryItemList = await _inventoryItemsRepository.GetAllAsync(item => item.UserId == userId);
            var itemIds = inventoryItemList.Select(item => item.CatalogItemId);
            var catalogItems = await _catalogItemsRepository.GetAllAsync(item => itemIds.Contains(item.Id));

            var inventoryItemsDto = inventoryItemList.Select(inventoryItem =>
            {
                var catalogItem = catalogItems.Single(catalogItem => catalogItem.Id == inventoryItem.CatalogItemId);
                return inventoryItem.AsDTO(catalogItem.Name, catalogItem.Description);
            });

            return Ok(inventoryItemsDto);
        }
        [HttpPost("PostInventoryAsync")]
        public async Task<ActionResult> PostAsync(GrantItemContract grantItemsContract)
        {
            var inventoryItem = await _inventoryItemsRepository.GetAsync(item => item.UserId == grantItemsContract.UserId
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

                await _inventoryItemsRepository.CreateAsync(inventoryItem);
            }
            else
            {
                inventoryItem.Quantity += grantItemsContract.Quantity;
                await _inventoryItemsRepository.UpdateAsync(inventoryItem);
            }

            return Ok();
        }
    }
}
