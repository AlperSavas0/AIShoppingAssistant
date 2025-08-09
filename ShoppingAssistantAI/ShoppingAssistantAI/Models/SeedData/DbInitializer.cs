using Microsoft.EntityFrameworkCore;
using ShoppingAssistantAI.Models.ContextClasses;
using ShoppingAssistantAI.Models.Entities;

namespace ShoppingAssistantAI.Models.SeedData
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (context.AppUsers.Any()) return;

            // 1️⃣ Kullanıcılar
            var users = new List<AppUser>
            {
                new AppUser { UserName = "ali", Email = "ali@example.com", Password = "123" },
                new AppUser { UserName = "ayse", Email = "ayse@example.com", Password = "123" },
                new AppUser { UserName = "veli", Email = "veli@example.com", Password = "123" }
            };
            context.AppUsers.AddRange(users);
            context.SaveChanges();

            // 2️⃣ Ürünler
            // ... diğer using ve namespace tanımlamaları aynı kalacak

            var products = new List<Product>
            {
                new Product { Name = "Un", Category = "Gıda", Price = 25.5m, Tags = "pasta,un" },
                new Product { Name = "Yumurta", Category = "Gıda", Price = 10.0m, Tags = "yumurta,pasta" },
                new Product { Name = "Mikser", Category = "Mutfak Gereçleri", Price = 300.0m, Tags = "mikser,pasta" },
                new Product { Name = "Mouse", Category = "Bilgisayar", Price = 120.0m, Tags = "mouse,bilgisayar" },
                new Product { Name = "Klavye", Category = "Bilgisayar", Price = 200.0m, Tags = "klavye,bilgisayar" },
                new Product { Name = "Saç Kurutma Makinesi", Category = "Kişisel Bakım", Price = 350.0m, Tags = "saç kurutma,bakım" },
                new Product { Name = "Şampuan", Category = "Kişisel Bakım", Price = 45.0m, Tags = "şampuan,bakım" },
                new Product { Name = "Tencere", Category = "Mutfak Gereçleri", Price = 180.0m, Tags = "tencere,mutfak" },
                new Product { Name = "Ampul", Category = "Ev Elektroniği", Price = 35.0m, Tags = "ampul,elektrik" },
                new Product { Name = "Çikolata", Category = "Gıda", Price = 15.0m, Tags = "çikolata,pasta" },
                // Devam edebilirsin (50'ye tamamlamak istersen)
            };

            context.Products.AddRange(products);
            context.SaveChanges();

            // 3️⃣ Siparişler
            var orders = new List<Order>
            {
                new Order { AppUser = users[0], CreatedDate = DateTime.Now.AddDays(-5) },
                new Order { AppUser = users[0], CreatedDate = DateTime.Now.AddDays(-2) },
                new Order { AppUser = users[1], CreatedDate = DateTime.Now.AddDays(-1) }
            };
            context.Orders.AddRange(orders);
            context.SaveChanges();

            // 4️⃣ Sipariş Detayları
            var orderDetails = new List<OrderDetail>
            {
                new OrderDetail
                {
                    Order = orders[0],
                    Product = products[1],
                    Amount = 2,
                    UnitPrice = products[1].Price
                },
                new OrderDetail
                {
                    Order = orders[0],
                    Product = products[5],
                    Amount = 1,
                    UnitPrice = products[5].Price
                },
                new OrderDetail
                {
                    Order = orders[1],
                    Product = products[9],
                    Amount = 3,
                    UnitPrice = products[9].Price
                },
                new OrderDetail
                {
                    Order = orders[2],
                    Product = products[3],
                    Amount = 1,
                    UnitPrice = products[3].Price
                }
            };
            context.OrderDetails.AddRange(orderDetails);
            context.SaveChanges();
        }
    }
}
