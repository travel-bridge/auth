using Auth.Infrastructure;
using Auth.Worker.Workers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHostedService<SendConfirmEmailWorker>();
builder.Services.AddHostedService<SendResetPasswordWorker>();
builder.Services.AddHealthChecks();

var app = builder.Build();
app.UseRouting();
app.MapHealthChecks("/health");

await app.RunAsync();