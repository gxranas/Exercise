namespace Exercise.Models
{
    public class OrderRequest
    {
        public List<string> Shipping_details { get; set; }
        public List<ProductRequest> Products { get; set; }
    }
}
