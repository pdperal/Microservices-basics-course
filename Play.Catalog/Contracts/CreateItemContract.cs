using System;
using System.ComponentModel.DataAnnotations;

namespace Play.Catalog.Service.Contracts
{
    public class CreateItemContract
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(0, 1000)]
        public decimal Price { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}