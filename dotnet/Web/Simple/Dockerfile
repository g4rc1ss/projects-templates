FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 7151
EXPOSE 5151

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
COPY . .
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build /p:CSharpier_Bypass=true

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SimpleWeb.HostWebApi/SimpleWeb.HostWebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false /p:CSharpier_Bypass=true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleWeb.HostWebApi.dll"]
