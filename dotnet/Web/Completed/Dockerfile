#if(UseGrpc)
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:9.0 AS base
#else
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
#endif
USER $APP_UID
WORKDIR /app
EXPOSE 7151
EXPOSE 5151

#if(UseGrpc)
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:9.0 AS build
#else
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
#endif
ARG BUILD_CONFIGURATION=Release
COPY . .
RUN dotnet build "CompletedWeb.HostWebApi/CompletedWeb.HostWebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build /p:CSharpier_Bypass=true

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CompletedWeb.HostWebApi/CompletedWeb.HostWebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false /p:CSharpier_Bypass=true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CompletedWeb.HostWebApi.dll"]
