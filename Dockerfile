#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Dws.Challenge.WebApi/Dws.Challenge.WebApi.csproj", "src/Dws.Challenge.WebApi/"]
COPY ["src/Dws.Challenge.Application/Dws.Challenge.Application.csproj", "src/Dws.Challenge.Application/"]
COPY ["src/Dws.Challenge.Infrastructure/Dws.Challenge.Infrastructure.csproj", "src/Dws.Challenge.Infrastructure/"]
COPY ["src/Dws.Challenge.Domain/Dws.Challenge.Domain.csproj", "src/Dws.Challenge.Domain/"]
RUN dotnet restore "src/Dws.Challenge.WebApi/Dws.Challenge.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Dws.Challenge.WebApi"
RUN dotnet build "Dws.Challenge.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dws.Challenge.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Dws.Challenge.WebApi.dll
#ENTRYPOINT ["dotnet", "Dws.Challenge.WebApi.dll"]