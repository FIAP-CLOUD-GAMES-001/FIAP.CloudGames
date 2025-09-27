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


# --- PASSO NOVO: Instala o agente do New Relic via APT (método mais robusto) ---
RUN apt-get update && apt-get install -y wget ca-certificates gnupg && \
    # Adiciona a chave de assinatura do repositório da New Relic
    wget -O /tmp/newrelic-key.gpg https://download.newrelic.com/548C16BF.gpg && \
    apt-key add /tmp/newrelic-key.gpg && \
    # Adiciona o repositório da New Relic à lista de fontes do APT
    echo "deb http://apt.newrelic.com/debian/ bookworm non-free" > /etc/apt/sources.list.d/newrelic.list && \
    # Atualiza a lista de pacotes novamente e instala o agente
    apt-get update && \
    apt-get install -y newrelic-dotnet-agent && \
    # Limpeza
    apt-get clean && rm -rf /var/lib/apt/lists/*

# --- MUDANÇA: Voltamos a ser o usuário sem privilégios ---
USER $APP_UID

# Copia os arquivos da sua aplicação já publicados
COPY --from=publish /app/publish .

# Ativa o profiler do New Relic com variáveis de ambiente
# O caminho de instalação via APT é diferente do manual
ENV CORECLR_ENABLE_PROFILING=1 \
    CORECLR_PROFILER={36032161-FFC0-4B61-B552-3A686F5C71A9} \
    CORECLR_PROFILER_PATH=/usr/local/newrelic-dotnet-agent/libNewRelicProfiler.so \
    NEW_RELIC_HOME=/usr/local/newrelic-dotnet-agent

ENTRYPOINT ["dotnet", "FIAP.CloudGames.Api.dll"]