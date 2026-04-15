# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DevUtilityHub.Api/DevUtilityHub.Api.csproj", "DevUtilityHub.Api/"]
RUN dotnet restore "./DevUtilityHub.Api/DevUtilityHub.Api.csproj"
COPY . .
WORKDIR "/src/DevUtilityHub.Api"
RUN dotnet build "./DevUtilityHub.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DevUtilityHub.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevUtilityHub.Api.dll"]

# Debug stage — builds with Debug config, installs vsdbg for VS Code remote debugging
FROM build AS debug-build
RUN dotnet build "./DevUtilityHub.Api.csproj" -c Debug -o /app/debug

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS debug
USER root
RUN apt-get update && apt-get install -y curl unzip && \
    curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg && \
    rm -rf /var/lib/apt/lists/*
WORKDIR /app
COPY --from=debug-build /app/debug .
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 8080
EXPOSE 4024
ENTRYPOINT ["dotnet", "DevUtilityHub.Api.dll"]