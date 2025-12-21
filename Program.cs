using Microsoft.EntityFrameworkCore;
using HeatExchangeCalculator.Data;
using HeatExchangeCalculator.Services;

var builder = WebApplication.CreateBuilder(args);

// Конфигурация базы данных SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация сервисов
builder.Services.AddScoped<HeatExchangeService>();

// Добавление контроллеров и представлений
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

// Добавление поддержки Razor Pages для валидации
builder.Services.AddRazorPages();

var app = builder.Build();

// Миграция базы данных при запуске
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
}

// Конфигурация пайплайна
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