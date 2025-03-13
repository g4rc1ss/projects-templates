using Projects;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Template_HostWebApi>("Template");

builder.Build().Run();