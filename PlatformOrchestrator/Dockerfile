FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["PlatformOrchestrator.Api/PlatformOrchestrator.Api.csproj", "PlatformOrchestrator.Api/"]
COPY ["PlatformOrchestrator.Core/PlatformOrchestrator.Core.csproj", "PlatformOrchestrator.Core/"]
COPY ["PlatformOrchestrator.Infrastructure/PlatformOrchestrator.Infrastructure.csproj", "PlatformOrchestrator.Infrastructure/"]
RUN dotnet restore "PlatformOrchestrator.Api/PlatformOrchestrator.Api.csproj"
COPY . .
WORKDIR "/src/PlatformOrchestrator.Api"
RUN dotnet build "PlatformOrchestrator.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PlatformOrchestrator.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PlatformOrchestrator.Api.dll"]
