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

# -----------------------------------------------------------------------------------
# Esta fase é usada na produção E AGORA INCLUI O NEW RELIC
FROM base AS final
WORKDIR /app

# --- PASSO NOVO: Instala o agente do New Relic ---
# Instala ferramentas, baixa o agente, extrai e limpa.
RUN apt-get update && apt-get install -y wget && \
    wget https://download.newrelic.com/dot_net_agent/latest_release/newrelic-agent-linux_10.25.0_amd64.tar.gz -O /tmp/newrelic.tar.gz && \
    mkdir -p /usr/local/newrelic-agent && \
    tar -C /usr/local/newrelic-agent -xzf /tmp/newrelic.tar.gz && \
    rm /tmp/newrelic.tar.gz

# Copia os arquivos da sua aplicação já publicados
COPY --from=publish /app/publish .

# --- PASSO NOVO: Ativa o profiler do New Relic com variáveis de ambiente ---
# A chave de licença e o nome da app serão fornecidos pela Azure Container Apps
ENV CORECLR_ENABLE_PROFILING=1 \
    CORECLR_PROFILER={36032161-FFC0-4B61-B552-3A686F5C71A9} \
    CORECLR_PROFILER_PATH=/usr/local/newrelic-agent/libNewRelicProfiler.so \
    NEW_RELIC_HOME=/usr/local/newrelic-agent

ENTRYPOINT ["dotnet", "FIAP.CloudGames.Api.dll"]