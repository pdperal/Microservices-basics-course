using MassTransit;
using Play.Catalog.Contracts.Contracts;
using Play.Common.Repositories;
using Play.Inventory.Entities;
using System;
using System.Threading.Tasks;

namespace Play.Inventory.Consumers
{
    public class CatalogItemDeletedConsumer : IConsumer<CatalogItemDeletedContract>
    {
        private readonly IRepository<CatalogItem> _repository;

        public CatalogItemDeletedConsumer(IRepository<CatalogItem> repository)
        {
            _repository = repository;
        }
        public async Task Consume(ConsumeContext<CatalogItemDeletedContract> context)
        {
            var message = context.Message;
            var item = await _repository.GetAsync(message.ItemId);

            if (item == null)
            {
                return;
            }

            await _repository.DeleteAsync(message.ItemId);
        }
    }
}
