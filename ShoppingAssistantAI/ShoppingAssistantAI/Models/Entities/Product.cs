namespace ShoppingAssistantAI.Models.Entities
{
    public class Product :BaseEntity
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Tags { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }


        //Relational Properties
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
