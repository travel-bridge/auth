using Auth.Services.Data;
using Auth.Services.Domain;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("AuthDatabase")
    ?? throw new InvalidOperationException("Connection string is not configured.");
builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(connectionString));

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

// TODO: Add Health Checks

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();
app.MapDefaultControllerRoute();
// TODO: Map Health Checks

await app.RunAsync();