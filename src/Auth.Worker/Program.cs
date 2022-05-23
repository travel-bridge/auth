using Auth.Worker.Events;
using Auth.Worker.Workers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOptions<KafkaOptions>()
    .Bind(builder.Configuration.GetSection(KafkaOptions.SectionKey))
    .ValidateDataAnnotations();

builder.Services.AddSingleton<IEventConsumerFactory, EventConsumerFactory>();
builder.Services.AddHostedService<SendConfirmEmailWorker>();
builder.Services.AddHostedService<SendResetPasswordWorker>();
builder.Services.AddHealthChecks();

var app = builder.Build();
app.UseRouting();
app.MapHealthChecks("/health");

await app.RunAsync();