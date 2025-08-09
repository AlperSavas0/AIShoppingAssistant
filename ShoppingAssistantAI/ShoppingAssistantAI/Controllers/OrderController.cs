using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingAssistantAI.Models.ContextClasses;
using ShoppingAssistantAI.Models.DTOs;
using ShoppingAssistantAI.Models.Entities;
using ShoppingAssistantAI.Services;

namespace ShoppingAssistantAI.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly GeminiService _gemini;
        public OrderController(AppDbContext context,GeminiService gemini)
        {
            _context = context;
            _gemini = gemini;
        }

        public IActionResult Index()
        {
            List<Order> orders = _context.Orders.Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Where(o => o.AppUserID == 1)
                .OrderByDescending(o => o.CreatedDate).ToList();

            return View(orders);
        }

        public IActionResult Details(int id)
        {
            Order? order = _context.Orders.Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefault(o => o.ID == id);

            if (order == null) return NotFound();

            return View(order);
        }

       
        [HttpGet]
        public async Task<IActionResult> ChatFindInOrders(string message)
        {
            LLMResult llmResult = await _gemini.AnalyzeMessageAsync(message);
            if (llmResult == null)
            {
                ViewBag.Error = "AI'dan anlamlı bir çıktı alınamadı.";
                return View("PastOrderResults", new List<PastOrderResult>());
            }

            // Normalize
            List<string> tags = (llmResult.tags ?? new List<string>())
                        .Select(t => t.ToLower().Trim())
                        .Where(t => !string.IsNullOrWhiteSpace(t))
                        .Distinct()
                        .ToList();

            List<string> categories = (llmResult.categories ?? new List<string>())
                        .Select(c => c.ToLower().Trim())
                        .Where(c => !string.IsNullOrWhiteSpace(c))
                        .Distinct()
                        .ToList();

            
            HashSet<string> keywords = new HashSet<string>(tags.Concat(categories));

            
            foreach (string token in (message ?? "").ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                if (token.Length >= 3) keywords.Add(token.Trim(',', '.', ';', ':', '!', '?'));
            }

            // Kullanıcının geçmiş siparişleri + ürünler
            List<Order> userOrders = _context.Orders
                .Where(o => o.AppUserID == 1)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .ToList(); // RAM'e al: LINQ to Objects

            // Eşleşen OrderDetail’leri topla
            var hits = userOrders
                .SelectMany(o => o.OrderDetails.Select(od => new { Order = o, Detail = od, Product = od.Product }))
                .Where(x =>
                    keywords.Any(k =>
                        (!string.IsNullOrEmpty(x.Product?.Tags) && x.Product.Tags.ToLower().Contains(k)) ||
                        (!string.IsNullOrEmpty(x.Product?.Name) && x.Product.Name.ToLower().Contains(k)) ||
                        (!string.IsNullOrEmpty(x.Product?.Category) && x.Product.Category.ToLower().Contains(k))
                    )
                )
                .ToList();

            List<PastOrderResult> grouped = hits
                .GroupBy(x => x.Product.ID)
                .Select(g => new PastOrderResult
                {
                    Product = g.First().Product,
                    LastPurchasedAt = g.Max(y => y.Order.CreatedDate),
                    TimesPurchased = g.Count(),
                    LastOrderID = g.OrderByDescending(y => y.Order.CreatedDate).First().Order.ID
                })
                .OrderByDescending(r => r.LastPurchasedAt)
                .ToList();

            if (!grouped.Any())
            {
                ViewBag.Error = "Geçmiş siparişlerinizde eşleşen ürün bulunamadı.";
            }

            // Debug
            ViewBag.RawTags = string.Join(", ", tags);
            ViewBag.RawCategories = string.Join(", ", categories);

            return View("PastOrderResults", grouped);
        }
    }

}

