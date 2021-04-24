using System;

namespace Play.Catalog.Service.DTO
{
    public record ItemDTO(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreatedDate);
}
