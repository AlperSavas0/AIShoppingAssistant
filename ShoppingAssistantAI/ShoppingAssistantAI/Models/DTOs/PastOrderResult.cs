using ShoppingAssistantAI.Models.Entities;

namespace ShoppingAssistantAI.Models.DTOs
{
    public class PastOrderResult
    {
        public Product Product{ get; set; }
        public DateTime LastPurchasedAt { get; set; }
        public int TimesPurchased { get; set; }
        public int LastOrderID { get; set; }

    }
}
