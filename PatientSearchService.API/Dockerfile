#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 30029 
EXPOSE 30039

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["PatientSearchService.API/PatientSearchService.API.csproj", "PatientSearchService.API/"]
COPY ["iPas.Infrastructure.EventBus/iPas.Infrastructure.EventBus.csproj", "iPas.Infrastructure.EventBus/"]
COPY ["PatientSearchService.Data/PatientSearchService.Data.csproj", "PatientSearchService.Data/"]
COPY ["iPas.Infrastructure.IntegrationEventLogEF/iPas.Infrastructure.IntegrationEventLogEF.csproj", "iPas.Infrastructure.IntegrationEventLogEF/"]
COPY ["iPas.Infrastructure.EventBusServiceBus/iPas.Infrastructure.EventBusServiceBus.csproj", "iPas.Infrastructure.EventBusServiceBus/"]
RUN dotnet restore "PatientSearchService.API/PatientSearchService.API.csproj"
COPY . .
WORKDIR "/src/PatientSearchService.API"
RUN dotnet build "PatientSearchService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PatientSearchService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PatientSearchService.API.dll"]