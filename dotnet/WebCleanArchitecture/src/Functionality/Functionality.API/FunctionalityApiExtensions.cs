﻿using Functionality.Application;
using Functionality.Infraestructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Functionality.API;

public static class FunctionalityApiExtensions
{
    public static void InitFunctionalityApi(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .AddApplicationPart(typeof(FunctionalityApiExtensions).Assembly);

        builder.Services.AddBusinessServices();
        builder.AddDataAccessService();
    }
}