using Microsoft.EntityFrameworkCore;
using HeatExchangeCalculator.Data;
using HeatExchangeCalculator.Services;

var builder = WebApplication.CreateBuilder(args);

// === БАЗА ДАННЫХ ТОЛЬКО В bin\Debug\net8.0\ ===
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    // Путь к папке где запущено приложение (bin\Debug\net8.0\)
    string appBaseDir = AppContext.BaseDirectory;
    string dbPath = Path.Combine(appBaseDir, "HeatExchangeCalculator.db");
    
    Console.WriteLine($"=== БАЗА ДАННЫХ ===");
    Console.WriteLine($"Запуск из: {appBaseDir}");
    Console.WriteLine($"Файл БД: {dbPath}");
    Console.WriteLine($"===================\n");
    
    options.UseSqlite($"Data Source={dbPath}");
});

builder.Services.AddScoped<HeatExchangeService>();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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