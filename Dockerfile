#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /src
COPY . .
COPY ["src/Campaign/Campaign.csproj", "src/Campaign/"]
COPY ["src/Service/Service.csproj", "src/Service/"]
COPY ["src/Model/Model.csproj", "src/Model/"]
COPY ["src/Data/Data.csproj", "src/Data/"]
RUN dotnet restore "src/Campaign/Campaign.csproj"
COPY ["src", "./"]

RUN dotnet build "src/Campaign/Campaign.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/Campaign/Campaign.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Campaign.dll"]