FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MusicBackendApp.API/MusicBackendApp.API.csproj", "MusicBackendApp.API/"]
COPY ["MusicBackendApp.Infrastructure/MusicBackendApp.Infrastructure.csproj", "MusicBackendApp.Infrastructure/"]
COPY ["MusicBackendApp.Application/MusicBackendApp.Application.csproj", "MusicBackendApp.Application/"]
COPY ["MusicBackendApp.Domain/MusicBackendApp.Domain.csproj", "MusicBackendApp.Domain/"]
RUN dotnet restore "MusicBackendApp.API/MusicBackendApp.API.csproj"
COPY . .
WORKDIR "/src/MusicBackendApp.API"
RUN dotnet build "MusicBackendApp.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MusicBackendApp.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MusicBackendApp.API.dll"]

# Stage 1: Build
#FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
#WORKDIR /app

# Copy all project files and restore dependencies
#COPY *.sln .
#COPY MusicBackendApp.Application/*.csproj ./MusicBackendApp.Application/
#COPY MusicBackendApp.API/*.csproj ./MusicBackendApp.API/
#COPY MusicBackendApp.Domain/*.csproj ./MusicBackendApp.Domain/
#COPY MusicBackendApp.Infrastructure/*.csproj ./MusicBackendApp.Infrastructure/
#RUN dotnet restore

# Copy all files and build the application
#COPY . .
#RUN dotnet build -c Release --no-restore

# Stage 2: Publish
#FROM build AS publish
#RUN dotnet publish -c Release -o /app/publish --no-build

# Stage 3: Final image
#FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "MusicBackendApp.API.dll"]