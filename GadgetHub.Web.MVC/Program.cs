using GadgetHub.Web.MVC.HttpClients;
using GadgetHub.Web.MVC.Interface;
using GadgetHub.Web.MVC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

// Common HttpClient configuration
void ConfigureApiClient(HttpClient client)
{
    client.BaseAddress = new Uri("https://localhost:44379/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
}

// Register API clients
builder.Services.AddHttpClient<IProductApiClient, ProductsApiClient>(ConfigureApiClient);
builder.Services.AddHttpClient<ICategoryApiClient, CategoriesApiClient>(ConfigureApiClient);
builder.Services.AddHttpClient<IRegisterApiClient, RegisterApiClient>(ConfigureApiClient);
builder.Services.AddHttpClient<ILoginApiClient, LoginApiClient>(ConfigureApiClient);
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

app.UseSession();

// Configure the HTTP request pipeline
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