using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Contracts;
using Play.Catalog.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("/api/items")]
    public class ItemsController : ControllerBase
    {
        private static readonly List<ItemDTO> _items = new List<ItemDTO>()
        {
            new ItemDTO(Guid.NewGuid(), "Potion", "Restores the small amount of HP", 5, DateTimeOffset.UtcNow),
            new ItemDTO(Guid.NewGuid(), "Antidote", "Cures poison.", 7, DateTimeOffset.UtcNow),
            new ItemDTO(Guid.NewGuid(), "Sword", "Deals a small amount of damage", 20, DateTimeOffset.UtcNow),
        };

        [HttpGet]
        public IEnumerable<ItemDTO> Get()
        {
            return _items;
        }

        [HttpGet("{id}")]

        public ActionResult<ItemDTO> GetById(Guid id)
        {
            var item = _items.Where(x => x.Id == id).SingleOrDefault();
            if (item == null)
            {
                return NotFound();
            }

            return item;  
        }

        [HttpPost]
        public ActionResult<ItemDTO> Post([FromBody]CreateItemContract createItemContract)
        {
            var item = new ItemDTO(Guid.NewGuid(), createItemContract.Name, createItemContract.Description, createItemContract.Price, createItemContract.CreatedAt);
            _items.Add(item);

            return CreatedAtRoute(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody]UpdateItemContract updateItemContract)
        {
            var existingItem = _items.Where(item => item.Id == id).SingleOrDefault();

            if (existingItem == null)
            {
                return NotFound();
            }

            var updatedItem = existingItem with
            {
                Name = updateItemContract.Name,
                Description = updateItemContract.Description,
                Price = updateItemContract.Price
            };

            var index = _items.FindIndex(existingItem => existingItem.Id == id);
            _items[index] = updatedItem;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            var index = _items.FindIndex(item => item.Id == id);

            if (index < 0)
            {
                return NotFound();
            }

            _items.RemoveAt(index);

            return NoContent();
        }
    }
}
