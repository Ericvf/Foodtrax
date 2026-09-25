using Foodtrax;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSingleton<SqlLiteService>()
    .AddScoped<FoodRepository>()
    .AddScoped<ConsumedFoodRepository>()
    .AddRazorComponents().AddInteractiveServerComponents();

var app = builder.Build();

app.UseHsts();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

using (var scope = app.Services.CreateScope())
{
    var sqlite = scope.ServiceProvider.GetRequiredService<SqlLiteService>();

    using var connection = sqlite.CreateConnection();
    connection.Open();

    await sqlite.Initialize();
}

await app.RunAsync();