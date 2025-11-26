using GadgetHub.Web.MVC.HttpClients;
using GadgetHub.Web.MVC.Interface;
using GadgetHub.Web.MVC.Services;

var builder = WebApplication.CreateBuilder(args);



// BUNDLING 
builder.Services.AddControllersWithViews();
builder.Services.AddWebOptimizer(pipeline => {
    pipeline.AddCssBundle("/css/bundle.css", "lib/bootstrap/dist/css/bootstrap.css", "css/site.css");
    pipeline.AddJavaScriptBundle("/js/bundle.js", "lib/jquery/dist/jquery.js", "js/site.js");
    pipeline.AddJavaScriptBundle("/js/validation.js",
    "lib/jquery-validation/dist/jquery.validate.js",
    "lib/jquery-validation-unobtrusive/js/jquery.validate.unobtrusive.js");

} );
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
    app.UseWebOptimizer();
}

app.UseHttpsRedirection();
app.UseWebOptimizer();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();