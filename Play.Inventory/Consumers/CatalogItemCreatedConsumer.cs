using MassTransit;
using Play.Catalog.Contracts.Contracts;
using Play.Common.Repositories;
using Play.Inventory.Entities;
using System.Threading.Tasks;

namespace Play.Inventory.Consumers
{
    public class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreatedContract>
    {
        private readonly IRepository<CatalogItem> _repository;

        public CatalogItemCreatedConsumer(IRepository<CatalogItem> repository)
        {
            _repository = repository;
        }
        public async Task Consume(ConsumeContext<CatalogItemCreatedContract> context)
        {
            var message = context.Message;
            var item = await _repository.GetAsync(message.ItemId);

            if (item != null)
            {
                return;
            }

            var catalogItem = new CatalogItem
            {
                Id = message.ItemId,
                Name = message.Name,
                Description = message.Description
            };

            await _repository.CreateAsync(catalogItem);
        }
    }
}
