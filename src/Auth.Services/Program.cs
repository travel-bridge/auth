using Auth.Services.Data;
using Auth.Services.Events;
using Auth.Services.IdentityProviders;
using Auth.Services.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("AuthDatabase")
    ?? throw new InvalidOperationException("Connection string is not configured.");

builder.Services.AddDbContext<DataContext>(
    x => x.UseNpgsql(
        connectionString,
        options => options.EnableRetryOnFailure(
            3,
            TimeSpan.FromSeconds(10),
            null)));

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

builder.Services.AddOptions<KafkaOptions>()
    .Bind(builder.Configuration.GetSection(KafkaOptions.SectionKey))
    .ValidateDataAnnotations();

builder.Services.AddSingleton<IEventProducer, EventProducer>();

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