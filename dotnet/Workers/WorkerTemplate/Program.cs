using ServiceDefaults;
using WorkerTemplate.Workers;
#if (UseRabbitMQ)
using WorkerTemplate.Workers.RabbitMq;
#endif
#if (UseAzServiceBus)
using WorkerTemplate.Workers.AzServiceBus;
#endif

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();
#if (UseRabbitMQ)
builder.AddRabbitMq();
#endif
#if (UseAzServiceBus)
builder.AddAzureServiceBus();
#endif

builder.AddServiceDefaults();

IHost host = builder.Build();


await host.RunAsync();