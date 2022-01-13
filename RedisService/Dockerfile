#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:7770
EXPOSE 7770

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src

COPY ["./RedisService.csproj", "RedisService/"]
RUN dotnet restore "RedisService/RedisService.csproj"
RUN echo "$PWD"
COPY . .
WORKDIR "/src/RedisService"
RUN dotnet build "RedisService.csproj" -c Release -o /app/build 

FROM build AS publish
RUN dotnet publish "RedisService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RedisService.dll"]