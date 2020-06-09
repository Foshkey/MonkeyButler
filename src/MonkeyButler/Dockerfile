#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/MonkeyButler/MonkeyButler.csproj", "src/MonkeyButler/"]
COPY ["src/MonkeyButler.Business/MonkeyButler.Business.csproj", "src/MonkeyButler.Business/"]
COPY ["src/MonkeyButler.Data/MonkeyButler.Data.csproj", "src/MonkeyButler.Data/"]
RUN dotnet restore "src/MonkeyButler/MonkeyButler.csproj"
COPY . .
WORKDIR "/src/src/MonkeyButler"
RUN dotnet build "MonkeyButler.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MonkeyButler.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MonkeyButler.dll"]