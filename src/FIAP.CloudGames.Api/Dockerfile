# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# --- MUDANÇA 1: Corrigido o caminho do .csproj e do .sln ---
# O caminho de origem agora inclui "src/" para que seja encontrado a partir da raiz do projeto.
COPY ["FIAP.CloudGames.sln", "."]
COPY ["src/FIAP.CloudGames.Api/FIAP.CloudGames.Api.csproj", "FIAP.CloudGames.Api/"]
RUN dotnet restore "FIAP.CloudGames.Api/FIAP.CloudGames.Api.csproj"

# --- MUDANÇA 2: Corrigida a cópia dos arquivos-fonte ---
# Copiamos todo o conteúdo da pasta 'src' local para a pasta '/src' do contêiner.
COPY ["src/.", "."]

WORKDIR "/src/FIAP.CloudGames.Api"
RUN dotnet build "FIAP.CloudGames.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FIAP.CloudGames.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FIAP.CloudGames.Api.dll"]