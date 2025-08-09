namespace ShoppingAssistantAI.Models.Entities
{
    public class OrderDetail :BaseEntity
    {
        public int Amount { get; set; }
        public decimal UnitPrice { get; set; }

        public int OrderID { get; set; }
        public int ProductID { get; set; }

        //Relational Properties
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
