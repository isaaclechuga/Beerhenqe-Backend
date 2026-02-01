# ============ Etapa 1: Build (.NET 5) ============
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

# Copiar todo y restaurar
COPY . .
RUN dotnet restore MVCconCapasGraphQL/MVCconCapasGraphQL.csproj

# Publicar
RUN dotnet publish MVCconCapasGraphQL/MVCconCapasGraphQL.csproj -c Release -o /app/publish

# ============ Etapa 2: Runtime (.NET 5) ============
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app/publish .

# Importante: en contenedor usaremos solo HTTP
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "MVCconCapasGraphQL.dll"]
