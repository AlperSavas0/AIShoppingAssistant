namespace ShoppingAssistantAI.Models.DTOs
{
    public class LLMResult
    {
        public string intent { get; set; }
        public List<string> categories { get; set; }
        public List<string> tags { get; set; }

    }
}
