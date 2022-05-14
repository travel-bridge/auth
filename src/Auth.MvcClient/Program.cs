var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "https://localhost:5001";
        options.ClientId = "b3221bd2-82c1-40b3-800d-51010cc1d4b1";
        options.ClientSecret = "ef295c8f-f102-4f1d-b8b4-318ce94569f8";
        options.ResponseType = "code";
        options.SaveTokens = true;
    });


var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();

await app.RunAsync();