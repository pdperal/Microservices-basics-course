using Microsoft.AspNetCore.Mvc;
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
        private static int _counter;
        private readonly IRepository<Item> _itemsRepository;

        public ItemsController(IRepository<Item> itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDTO>>> GetAsync()
        {
            _counter++;
            Console.WriteLine($"Request {_counter}: Starting....");
            
            if(_counter <=2)
            {
                Console.WriteLine($"Request {_counter}: Starting....");
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
            if (_counter <= 4)
            {
                Console.WriteLine($"Request {_counter}: Status 500 (Internal Server Error).");
                return StatusCode(500);
            }


            var items = (await _itemsRepository.GetAllAsync())
                .Select(x => x.AsDTO());

            Console.WriteLine($"Request {_counter}: Status 200 (Ok).");

            return Ok(items);
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

            return NoContent();
        }
    }
}
