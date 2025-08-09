namespace ShoppingAssistantAI.Models.Entities
{
    public class AppUser :BaseEntity
    {
        
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        //Relational Properties
        public virtual ICollection<Order> Orders { get; set; }
    }
}
