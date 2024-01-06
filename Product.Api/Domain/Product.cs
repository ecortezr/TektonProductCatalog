using System.ComponentModel.DataAnnotations;

namespace Product.Api.Domain
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public bool Status { get; set; } = true;
        public int Stock { get; set; }
        public decimal Price { get; set; }
    }
}
