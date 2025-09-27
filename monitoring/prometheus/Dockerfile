FROM prom/prometheus:latest

# Copia o prometheus.yml para a pasta padrão do Prometheus
COPY prometheus.yml /etc/prometheus/prometheus.yml

# Expõe a porta padrão do Prometheus
EXPOSE 9090

# Comando padrão para iniciar o Prometheus
CMD [ "--config.file=/etc/prometheus/prometheus-prod.yml" ]