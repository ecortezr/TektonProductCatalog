using Product.Api.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Product.Api.Domain.Entities
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public ProductStatus Status { get; set; } = ProductStatus.Active;
        public int Stock { get; set; }
        public decimal Price { get; set; }
    }
}
