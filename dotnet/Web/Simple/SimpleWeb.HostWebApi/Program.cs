using SimpleWeb.HostWebApi.Extensions;
using SimpleWeb.HostWebApi.OpenAPI;
#if (UseMvc)
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using SimpleWeb.HostWebApi.Configurations;
#endif

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options => options.Limits.MaxRequestBodySize = 5 * 1024 * 1024);

builder.Configuration.AddUserSecrets<Program>().AddEnvironmentVariables();

// Add services to the container.
builder.InitWebHostConfig();

#if (UseGrpc)
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
#endif

#if (UseMvc)
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new KebabCaseTransformer()));
});
#endif

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.InitAndConfigureSwagger();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();
if (!app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
#if (UseGrpc)
    app.MapGrpcReflectionService();
#endif
}

app.UseHttpsRedirection();

app.MapHealthChecks("/health");
app.MapControllers();

#if (UseGrpc)
app.MapGrpcHealthChecksService();
#endif
app.MapRouteServices();

await app.RunAsync();
