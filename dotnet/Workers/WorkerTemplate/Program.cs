using WorkerTemplate.Workers;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();
builder.Services.AddHostedService<RabbitMqWorker>();
builder.Services.AddHostedService<AzServiceBusWorker>();


IHost host = builder.Build();
host.Run();