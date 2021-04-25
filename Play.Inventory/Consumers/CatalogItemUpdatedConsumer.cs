using MassTransit;
using Play.Catalog.Contracts.Contracts;
using Play.Common.Repositories;
using Play.Inventory.Entities;
using System;
using System.Threading.Tasks;

namespace Play.Inventory.Consumers
{
    public class CatalogItemUpdatedConsumer : IConsumer<CatalogItemUpdatedContract>
    {
        private readonly IRepository<CatalogItem> _repository;

        public CatalogItemUpdatedConsumer(IRepository<CatalogItem> repository)
        {
            _repository = repository;
        }
        public async Task Consume(ConsumeContext<CatalogItemUpdatedContract> context)
        {
            var message = context.Message;
            var item = await _repository.GetAsync(message.ItemId);

            if (item == null)
            {
                var catalogItem = new CatalogItem
                {
                    Id = message.ItemId,
                    Name = message.Name,
                    Description = message.Description
                };

                await _repository.CreateAsync(catalogItem);
            }
            else
            {
                item.Name = message.Name;
                item.Description = message.Description;

                await _repository.UpdateAsync(item);
            }
        }
    }
}
