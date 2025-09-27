# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# A imagem base já define o usuário não-root.
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia os arquivos de projeto/solução primeiro para otimizar o cache do Docker
COPY ["FIAP.CloudGames.sln", "."]
COPY ["src/FIAP.CloudGames.Api/FIAP.CloudGames.Api.csproj", "FIAP.CloudGames.Api/"]
RUN dotnet restore "FIAP.CloudGames.Api/FIAP.CloudGames.Api.csproj"

# Copia o restante do código-fonte
COPY ["src/.", "."]

WORKDIR "/src/FIAP.CloudGames.Api"
RUN dotnet build "FIAP.CloudGames.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FIAP.CloudGames.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# -----------------------------------------------------------------------------------
# Esta fase é usada na produção E AGORA INCLUI O NEW RELIC com a correção de permissão
FROM base AS final
WORKDIR /app

# --- MUDANÇA: Viramos o superusuário (root) temporariamente para poder instalar pacotes ---
USER root

# --- PASSO NOVO: Instala o agente do New Relic ---
# Agora este comando tem permissão para rodar.
RUN apt-get update && apt-get install -y wget ca-certificates && \
    # --- MUDANÇA AQUI: Usamos o link para a versão mais recente ---
    wget -O /tmp/newrelic-agent.tar.gz "https://download.newrelic.com/dot_net_agent/latest_release/newrelic-dotnet-agent-linux-x64.tar.gz" && \
    mkdir -p /usr/local/newrelic-agent && \
    tar -C /usr/local/newrelic-agent -xzf /tmp/newrelic-agent.tar.gz && \
    rm /tmp/newrelic-agent.tar.gz

# --- MUDANÇA: Voltamos a ser o usuário sem privilégios ($APP_UID) por segurança ---
USER $APP_UID

# Copia os arquivos da sua aplicação já publicados
COPY --from=publish /app/publish .

# Ativa o profiler do New Relic com variáveis de ambiente
ENV CORECLR_ENABLE_PROFILING=1 \
    CORECLR_PROFILER={36032161-FFC0-4B61-B552-3A686F5C71A9} \
    CORECLR_PROFILER_PATH=/usr/local/newrelic-agent/libNewRelicProfiler.so \
    NEW_RELIC_HOME=/usr/local/newrelic-agent

ENTRYPOINT ["dotnet", "FIAP.CloudGames.Api.dll"]