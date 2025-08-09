# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar todos los archivos del proyecto
COPY . .

# Restaurar dependencias desde el proyecto raíz
RUN dotnet restore ./FinanceService.csproj

# Publicar en modo release
RUN dotnet publish ./FinanceService.csproj -c Release -o /publish

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /publish .

# Expone el puerto HTTP por defecto
EXPOSE 80

ENTRYPOINT ["dotnet", "FinanceService.dll"]
