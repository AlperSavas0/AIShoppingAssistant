using Microsoft.EntityFrameworkCore;
using ShoppingAssistantAI.Models.ContextClasses;
using ShoppingAssistantAI.Models.SeedData;
using ShoppingAssistantAI.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<GeminiService>();

builder.Services.AddDbContextPool<AppDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")));



WebApplication app = builder.Build();



using (IServiceScope scope = app.Services.CreateScope())
{
    AppDbContext db=scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    DbInitializer.Seed(db);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
