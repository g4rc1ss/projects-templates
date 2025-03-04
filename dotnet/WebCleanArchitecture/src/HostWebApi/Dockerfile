FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/HostWebApi/HostWebApi.csproj", "src/HostWebApi/"]
COPY ["src/Template/Template.Shared/Template.Shared.csproj", "src/Template/Template.Shared/"]
COPY ["src/Infraestructure.Database/Infraestructure.Database.csproj", "src/Infraestructure.Database/"]
COPY ["src/Infraestructure.Events/Infraestructure.Events.csproj", "src/Infraestructure.Events/"]
COPY ["src/Template/Template.Infraestructure/Template.Infraestructure.csproj", "src/Template/Template.Infraestructure/"]
COPY ["src/Template/Template.Interfaces/Template.Interfaces.csproj", "src/Template/Template.Interfaces/"]
COPY ["src/Template/Template.Domain/Template.Domain.csproj", "src/Template/Template.Domain/"]
COPY ["src/Template/Template.Application/Template.Application.csproj", "src/Template/Template.Application/"]
COPY ["src/Template/Template.API/Template.API.csproj", "src/Template/Template.API/"]
RUN dotnet restore "src/HostWebApi/HostWebApi.csproj"
COPY . .
WORKDIR "/src/src/HostWebApi"
RUN dotnet build "HostWebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HostWebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HostWebApi.dll"]
