using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Contracts.Contracts;
using Play.Catalog.Service.Contracts;
using Play.Catalog.Service.DTO;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Utils;
using Play.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("api/items")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Item> _itemsRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public ItemsController(IRepository<Item> itemsRepository, IPublishEndpoint publishEndpoint)
        {
            _itemsRepository = itemsRepository;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDTO>> GetAsync()
        {
            var items = (await _itemsRepository.GetAllAsync())
                .Select(x => x.AsDTO());

            return items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDTO>> GetByIdAsync(Guid id)
        {
            var item = await _itemsRepository.GetAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item.AsDTO();  
        }

        [HttpPost]
        public async Task<ActionResult<ItemDTO>> PostAsync([FromBody]CreateItemContract createItemContract)
        {
            var item = new Item
            {
                Name = createItemContract.Name,
                Description = createItemContract.Description,
                Price = createItemContract.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            await _itemsRepository.CreateAsync(item);

            await _publishEndpoint.Publish(new CatalogItemCreatedContract(item.Id, item.Name, item.Description));

            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, [FromBody]UpdateItemContract updateItemContract)
        {
            var existingItem = await _itemsRepository.GetAsync(id);

            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.Name = updateItemContract.Name;
            existingItem.Description = updateItemContract.Description;
            existingItem.Price = updateItemContract.Price;

            await _itemsRepository.UpdateAsync(existingItem);

            await _publishEndpoint.Publish(new CatalogItemUpdatedContract(existingItem.Id, existingItem.Name, existingItem.Description));

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            var item = await _itemsRepository.GetAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            await _itemsRepository.DeleteAsync(item.Id);

            await _publishEndpoint.Publish(new CatalogItemDeletedContract(id));

            return NoContent();
        }
    }
}
