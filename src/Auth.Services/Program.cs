using Auth.Infrastructure;
using Auth.Infrastructure.Database;
using Auth.Services.IdentityProviders;
using Auth.Services.Infrastructure;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

var identityBuilder = builder.Services
    .AddIdentityServer(options =>
    {
        options.Events.RaiseErrorEvents = true;
        options.Events.RaiseInformationEvents = true;
        options.Events.RaiseFailureEvents = true;
        options.Events.RaiseSuccessEvents = true;
        options.EmitStaticAudienceClaim = true;
    })
    .AddInMemoryIdentityResources(IdentityResourceProvider.GetIdentityResources())
    .AddInMemoryApiResources(ApiResourceProvider.GetApiResources())
    .AddInMemoryApiScopes(ApiScopeProvider.GetApiScopes())
    .AddInMemoryClients(builder.Configuration.GetSection("Identity:Clients")
        ?? throw new InvalidOperationException("Identity clients are not configured."))
    .AddAspNetIdentity<User>();

// TODO: Configure production SigningCredential
identityBuilder.AddDeveloperSigningCredential();

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("AuthDatabase")
        ?? throw new InvalidOperationException("Connection string is not configured."));

var app = builder.Build();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<NotFoundHandlerMiddleware>();
app.UseStaticFiles();
app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();
app.MapDefaultControllerRoute();
app.UseHealthChecks("/health");

await app.RunAsync();