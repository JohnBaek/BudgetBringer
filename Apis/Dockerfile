﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80
EXPOSE 81

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Apis/Apis.csproj", "Apis/"]
COPY ["Features/Features.csproj", "Features/"]
COPY ["Providers/Providers.csproj", "Providers/"]
COPY ["Models/Models.csproj", "Models/"]
RUN dotnet restore "Apis/Apis.csproj"
COPY . .
WORKDIR "/src/Apis"
RUN dotnet build "Apis.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Apis.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Apis.dll"]
