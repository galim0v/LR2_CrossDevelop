namespace Galimov_CrossPlatform_LR2_BackEnd.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int SellerId { get; set; }

        public bool IsAffordable(decimal budget)
        {
            return Price <= budget;
        }
    }
}
