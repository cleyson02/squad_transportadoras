
# build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o csproj primeiro para aproveitar o cache de restore.
COPY ["backend.csproj", "./"]
RUN dotnet restore "backend.csproj"

# Copia o restante do codigo e publica em modo Release.
COPY . .
RUN dotnet publish "backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

# imagem final (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# A API escuta na porta 8080 dentro do container.
EXPOSE 8080

# Copia apenas o resultado publicado (sem SDK, sem codigo-fonte).
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "backend.dll"]
