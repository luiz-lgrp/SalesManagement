# Etapa 1: Base (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Etapa 2: Build (SDK)
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copia o nuget.config para o local do NuGet
COPY nuget.config /root/.nuget/NuGet/NuGet.Config

# Copia os arquivos de projeto (csproj) para cada camada com os nomes corretos
COPY ["src/TestingCRUD.API/", "TestingCRUD.API/"]
COPY ["src/TestingCRUD.Aplication/", "TestingCRUD.Aplication/"]
COPY ["src/TestingCRUD.Domain/", "TestingCRUD.Domain/"]
COPY ["src/TestingCRUD.Infra/", "TestingCRUD.Infra/"]

# Restaura as dependências
RUN dotnet restore "TestingCRUD.API/SalesManagement.API.csproj" --configfile /root/.nuget/NuGet/NuGet.Config --force --disable-parallel

# Copia o restante do código
COPY . .

# Compila o projeto
WORKDIR /src/TestingCRUD.API
RUN dotnet build "SalesManagement.API.csproj" --no-restore -c Release -o /app/build

# Etapa 3: Publicação
FROM build AS publish
WORKDIR /src/TestingCRUD.API
RUN dotnet publish "SalesManagement.API.csproj" --no-restore -c Release -o /app/publish

# Etapa 4: Runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestingCRUD.API.dll"]
