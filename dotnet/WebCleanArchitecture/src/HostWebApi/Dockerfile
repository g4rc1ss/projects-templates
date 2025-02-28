FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/HostWebApi/HostWebApi.csproj", "src/HostWebApi/"]
COPY ["src/WebCleanArchitecture/WebCleanArchitecture.Shared/WebCleanArchitecture.Shared.csproj", "src/WebCleanArchitecture/WebCleanArchitecture.Shared/"]
COPY ["src/Infraestructure.Database/Infraestructure.Database.csproj", "src/Infraestructure.Database/"]
COPY ["src/Infraestructure.Events/Infraestructure.Events.csproj", "src/Infraestructure.Events/"]
COPY ["src/WebCleanArchitecture/WebCleanArchitecture.Infraestructure/WebCleanArchitecture.Infraestructure.csproj", "src/WebCleanArchitecture/WebCleanArchitecture.Infraestructure/"]
COPY ["src/WebCleanArchitecture/WebCleanArchitecture.Interfaces/WebCleanArchitecture.Interfaces.csproj", "src/WebCleanArchitecture/WebCleanArchitecture.Interfaces/"]
COPY ["src/WebCleanArchitecture/WebCleanArchitecture.Domain/WebCleanArchitecture.Domain.csproj", "src/WebCleanArchitecture/WebCleanArchitecture.Domain/"]
COPY ["src/WebCleanArchitecture/WebCleanArchitecture.Application/WebCleanArchitecture.Application.csproj", "src/WebCleanArchitecture/WebCleanArchitecture.Application/"]
COPY ["src/WebCleanArchitecture/WebCleanArchitecture.API/WebCleanArchitecture.API.csproj", "src/WebCleanArchitecture/WebCleanArchitecture.API/"]
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
