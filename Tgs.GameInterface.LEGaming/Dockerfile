#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.GameInterface.LEGaming/Tgs.GameInterface.LEGaming.csproj", "Tgs.GameInterface.LEGaming/"]
COPY ["Tgs.GameInterface.LEGaming.Core/Tgs.GameInterface.LEGaming.Core.csproj", "Tgs.GameInterface.LEGaming.Core/"]
COPY ["Tgs.GameInterface.LEGaming.Protos/Tgs.GameInterface.LEGaming.Protos.csproj", "Tgs.GameInterface.LEGaming.Protos/"]
RUN dotnet restore "./Tgs.GameInterface.LEGaming/Tgs.GameInterface.LEGaming.csproj"
COPY . .
WORKDIR "/src/Tgs.GameInterface.LEGaming"
RUN dotnet build "./Tgs.GameInterface.LEGaming.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tgs.GameInterface.LEGaming.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GameInterface.LEGaming.dll"]