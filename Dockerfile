FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MotoForce-api/MotoForce-api.csproj", "MotoForce-api/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]
COPY ["Interface/Interface.csproj", "Interface/"]
COPY ["Logic/Logic.csproj", "Logic/"]
RUN dotnet restore "MotoForce-api/MotoForce-api.csproj"
COPY . .
WORKDIR "/src/MotoForce-api"
RUN dotnet build "MotoForce-api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MotoForce-api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "MotoForce-api.dll"]
