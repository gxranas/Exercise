using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Exercise.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ShippingInfoId { get; set; }
        public ShippingInfo ShippingInfo { get; set; }
        public List<Products> Products { get; set; } = new List<Products>();
    }
}
