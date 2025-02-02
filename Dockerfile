# Etapa base para rodar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar o arquivo .csproj e restaurar as dependências
COPY ["FmsWebScrapingApi/FmsWebScrapingApi.csproj", "FmsWebScrapingApi/"]
RUN dotnet restore "FmsWebScrapingApi/FmsWebScrapingApi.csproj"

# Copiar todo o código e publicar
COPY . .
WORKDIR "/src/FmsWebScrapingApi"
RUN dotnet publish "FmsWebScrapingApi.csproj" -c Release -o /app/publish

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
RUN ls /app
CMD ["sh", "-c", "ls /app && dotnet FmsWebScrapingApi.dll"]
