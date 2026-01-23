FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5000

# --- INÍCIO DA INSTALAÇÃO DO NEW RELIC ---
# Instala dependências e o Agente New Relic (requer root)
RUN apt-get update && apt-get install -y wget ca-certificates gnupg \
    && echo 'deb http://apt.newrelic.com/debian/ newrelic non-free' | tee /etc/apt/sources.list.d/newrelic.list \
    && wget -O- https://download.newrelic.com/548C16BF.gpg | apt-key add - \
    && apt-get update \
    && apt-get install -y newrelic-dotnet-agent \
    && rm -rf /var/lib/apt/lists/*

# Habilita o Profiler do .NET para o New Relic
ENV CORECLR_ENABLE_PROFILING=1 \
    CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
    CORECLR_PROFILER_PATH=/usr/local/newrelic-dotnet-agent/libNewRelicProfiler.so \
    NEW_RELIC_APP_NAME="Tech.Challenge.API" \
    # A variável NEW_RELIC_LICENSE_KEY será injetada pelo Kubernetes
    NEW_RELIC_DISTRIBUTED_TRACING_ENABLED=true
# --- FIM DA INSTALAÇÃO DO NEW RELIC ---

ENV ASPNETCORE_URLS=http://+:5000

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release

WORKDIR /src

COPY ["Tech.Challenge/Tech.Challenge.csproj", "Tech.Challenge/"]
COPY ["Tech.Challenge.Domain/Tech.Challenge.Domain.csproj", "Tech.Challenge.Domain/"]
COPY ["Tech.Challenge.Application/Tech.Challenge.Application.csproj", "Tech.Challenge.Application/"]
COPY ["Tech.Challenge.Presentation/Tech.Challenge.Presentation.csproj", "Tech.Challenge.Presentation/"]
COPY ["Tech.Challenge.Infra.Database/Tech.Challenge.Infra.Database.csproj", "Tech.Challenge.Infra.Database/"]

RUN dotnet restore "Tech.Challenge/Tech.Challenge.csproj"
COPY . . 
WORKDIR "/src/Tech.Challenge"
RUN dotnet build "Tech.Challenge.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Tech.Challenge.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tech.Challenge.dll"]