using System.ComponentModel.DataAnnotations;

namespace MultiTenantDemo2.DAL
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50), Required]
        public string Name { get; set; }

        [StringLength(50)]
        public string Category { get; set; }

        public decimal? Price { get; set; }
    }
}
