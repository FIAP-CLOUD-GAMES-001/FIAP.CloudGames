# FIAP.CloudGames 🎮

**FIAP.CloudGames** é uma plataforma de venda de jogos digitais e gestão de servidores para partidas online, desenvolvida como parte do Tech Challenge da FIAP. Nesta primeira fase, o foco é criar um serviço de **cadastro de usuários** e **biblioteca de jogos adquiridos**, que servirá de base para futuras funcionalidades, como matchmaking e gerenciamento de servidores.

O objetivo é desenvolver uma **API REST em .NET 8** para gerenciar usuários e seus jogos, garantindo persistência de dados, qualidade de software e boas práticas de desenvolvimento.

## 📋 Objetivo do Projeto

Desenvolver uma API REST que permita:
- Cadastro e autenticação de usuários.

## 🛠️ Funcionalidades Principais

- **Cadastro de Usuários**: Validação de e-mail e senha (mínimo de 8 caracteres com números, letras e caracteres especiais).
- **Autenticação e Autorização**: Via token JWT com dois níveis de acesso:
  - **Usuário**: Acesso à plataforma e biblioteca de jogos.
  - **Administrador**: Cadastro de jogos, administração de usuários e criação de promoções.
- **Persistência de Dados**: Utilizando Entity Framework Core.
- **Desenvolvimento da API**: Seguindo padrão Controllers MVC, com middleware para tratamento de erros e logs.
- **Documentação**: Swagger para expor os endpoints da API.

## 📦 Estrutura do Projeto

- `FIAP.CloudGames.Api`: API principal.
- `FIAP.CloudGames.Domain`: Entidades e interfaces de domínio.
- `FIAP.CloudGames.Infrastructure`: Persistência e repositórios.
- `FIAP.CloudGames.Service`: Regras de negócio.

## 🚀 Como Rodar Localmente

### 1. Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

### 2. Subir o Banco de Dados da aplicação e para logs

```bash
docker-compose up -d
```

O docker compose vai subir um banco SqlServer e mongoDb automaticamente no docker com as seguintes configurações de acesso:

SqlServer:
```bash
Server=host.docker.internal,1433;Database=CloudGames;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;
```
MongoDB:
```bash
mongodb://host.docker.internal:27017/logsdb
```

Segue trecho formal e direto para inclusão no **README**:

---

### 3. Configuração de Segredos Locais

Para garantir a segurança dos dados sensíveis do administrador inicial, as credenciais de seed devem ser armazenadas em um arquivo **não versionado** chamado `appsettings.Secrets.json`.

Siga os passos abaixo:

1️. Copie o arquivo de exemplo:

```bash
cp appsettings.Secrets.json.example appsettings.Secrets.json
```

2️. Edite o arquivo `appsettings.Secrets.json` com os dados reais do usuário administrador:

```json
{
  "SeedAdmin": {
    "Email": "replace-with-admin-email@domain.com",
    "Password": "replace-with-strong-password"
  }
}
```

**Importante:** O arquivo `appsettings.Secrets.json` está no `.gitignore` e **não deve ser versionado**.

Dessa forma, cada desenvolvedor local poderá configurar suas próprias credenciais de seed de forma segura, sem risco de vazamento no repositório.

---

### 4. Executar a API

```bash
dotnet run --project ../FIAP.CloudGames.Api
```

## 📝 Notas

- Certifique-se de que o Docker está em execução antes de subir o banco de dados.
- As migrações são armazenadas em `FIAP.CloudGames.Infrastructure/Migrations` e aplicadas automaticamente ao iniciar o projeto.
- Para gerar scripts SQL, utilize o comando `dotnet ef migrations script`.
