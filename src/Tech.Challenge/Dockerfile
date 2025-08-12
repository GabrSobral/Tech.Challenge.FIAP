FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5000

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