var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = "cookies";
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("cookies")
    .AddOpenIdConnect("oidc", options =>
    {
        options.RequireHttpsMetadata = true;
        options.Authority = "https://localhost:5001";
        options.ClientId = "b3221bd2-82c1-40b3-800d-51010cc1d4b1";
        options.ClientSecret = "ef295c8f-f102-4f1d-b8b4-318ce94569f8";
        options.ResponseType = "code";

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");
        options.Scope.Add("phone");
        
        options.SaveTokens = true;
    });

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();

await app.RunAsync();