#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY . .
COPY ["src/Api/Api.csproj", "src/Api/"]
COPY ["src/Service/Service.csproj", "src/Service/"]
COPY ["src/Model/Model.csproj", "src/Model/"]
COPY ["src/Data/Data.csproj", "src/Data/"]
RUN dotnet restore "src/Api/Api.csproj"
COPY ["src", "./"]

RUN dotnet build "src/Api/Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/Api/Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]