using System.ComponentModel.DataAnnotations;

namespace Product.Api.Domain
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
    }
}
