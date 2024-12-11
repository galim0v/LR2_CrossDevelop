namespace Galimov_CrossPlatform_LR2_BackEnd.Models
{
    public class Seller
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();

        public void AddProduct(Product product)
        {
            product.SellerId = Id;
            Products.Add(product);
        }
    }
}
