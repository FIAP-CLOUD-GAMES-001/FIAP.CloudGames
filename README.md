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

### 2. Subir o Banco de Dados

```bash
docker-compose up -d
```

### 3. Gerar Migrations

Substitua `<MigrationName>` por um nome descritivo (ex: `InitialCreate`):

```bash
dotnet ef migrations add <MigrationName> --project ../FIAP.CloudGames.Infrastructure --startup-project ../FIAP.CloudGames.Api
```

### 4. Atualizar o Banco de Dados

```bash
dotnet ef database update --context DataContext --startup-project ../FIAP.CloudGames.Api
```

### 5. Executar a API

```bash
dotnet run --project ../FIAP.CloudGames.Api
```

## 📝 Notas

- Certifique-se de que o Docker está em execução antes de subir o banco de dados.
- As migrações são armazenadas em `FIAP.CloudGames.Infrastructure/Migrations`.
- Para gerar scripts SQL, utilize o comando `dotnet ef migrations script`.