using Microsoft.AspNetCore.Mvc;
using ShoppingAssistantAI.Models.ContextClasses;
using ShoppingAssistantAI.Models.DTOs;
using ShoppingAssistantAI.Models.Entities;
using ShoppingAssistantAI.Services;

namespace ShoppingAssistantAI.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly GeminiService _gemini;
        public ProductController(AppDbContext context, GeminiService gemini)
        {
            _context = context;
            _gemini = gemini;
        }

        public IActionResult Index(string tag)
        {
            List<Product> products = string.IsNullOrEmpty(tag)
                ? _context.Products.ToList()
                : _context.Products
                  .Where(p => p.Tags.Contains(tag))
                  .ToList();

            ViewBag.CurrentTag = tag;
            return View(products);
        }

        public IActionResult Details(int id)
        {
            Product? product = _context.Products.FirstOrDefault(p => p.ID == id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> ChatSearch(string message)
        {
            LLMResult llmResult = await _gemini.AnalyzeMessageAsync(message);

            if (llmResult != null && string.Equals(llmResult.intent, "past_order_search", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("ChatFindInOrders", "Order", new { message });
            }

            if (llmResult == null || (llmResult.tags.Count == 0 && llmResult.categories.Count == 0))
            {
                ViewBag.Error = "AI'dan anlamlı bir çıktı alınamadı.";
                return View("ProductList", new List<Product>());
            }

            // AI'dan gelen tag ve kategori verilerini normalize et
            List<string> tags = llmResult.tags.Select(t => t.ToLower().Trim()).ToList();
            List<string> categories = llmResult.categories.Select(c => c.ToLower().Trim()).ToList();

            ViewBag.RawTags = string.Join(", ", tags);
            ViewBag.RawCategories = string.Join(", ", categories);

            // Ürünleri belleğe al (EF Core string.Split desteklemediği için)
            List<Product> allProducts = _context.Products.ToList();

            // Eşleşen ürünleri filtrele
            List<Product> filteredProducts = allProducts.Where(p =>
            {
                IEnumerable<string> productTags = (p.Tags ?? "")
                    .ToLower()
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(t => t.Trim());

                return tags.Any(tag => productTags.Contains(tag)) ||
                       categories.Any(cat => productTags.Contains(cat));
            }).ToList();

            if (!filteredProducts.Any())
            {
                ViewBag.Error = "AI başarılı şekilde çalıştı ancak eşleşen ürün bulunamadı.";
            }

            return View("ProductList", filteredProducts);
        }

    }
}
