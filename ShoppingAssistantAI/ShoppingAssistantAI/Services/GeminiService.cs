using ShoppingAssistantAI.Models.DTOs;

namespace ShoppingAssistantAI.Services
{
    public class GeminiService
    {
        private readonly HttpClient _httpclient;

        public GeminiService(HttpClient httpclient)
        {
            _httpclient = httpclient;
        }

        public async Task<LLMResult> AnalyzeMessageAsync(string message)
        {
            var request = new { message = message };

            HttpResponseMessage response = await _httpclient.PostAsJsonAsync("http://localhost:8000/analyze/", request);

            if (response.IsSuccessStatusCode)
            {
                LLMResult? result = await response.Content.ReadFromJsonAsync<LLMResult>();
                return result;
            }

            return null;
        }
    }
}
