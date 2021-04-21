using System.ComponentModel.DataAnnotations;

namespace Play.Catalog.Service.DTO
{
    public class UpdateItemContract
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(0, 1000)]
        public decimal Price { get; set; }
    }
}
