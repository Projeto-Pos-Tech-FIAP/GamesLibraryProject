# TechChallenge Fase 1 - Games Library

API desenvolvida para o Tech Challenge Fase 1, com foco em gerenciamento de biblioteca de jogos, usuarios, pedidos, autenticacao e auditoria.

O projeto foi construido em .NET 8 seguindo uma separacao em camadas, com API REST, consulta GraphQL para jogos, autenticacao via Keycloak, persistencia em SQL Server, auditoria em MongoDB e cache distribuido com Redis.

## Tecnologias

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- MongoDB
- Redis
- Keycloak
- HotChocolate GraphQL
- Swagger/OpenAPI
- Serilog
- xUnit, Moq e EF Core InMemory
- Docker Compose

## Estrutura do projeto

```text
src/
  TechChallengeFase1.Api/             API, controllers, Swagger, GraphQL e middlewares
  TechChallengeFase1.Application/     Casos de uso, services, DTOs e mapeamentos
  TechChallengeFase1.Domain/          Entidades, enums, contratos e regras de dominio
  TechChallengeFase1.Infrastructure/  Repositorios, EF Core, Keycloak, MongoDB e cache
  TechChallengeFase1.Tests/           Testes automatizados

docker/
  keycloak/                           Realm exportado para importacao automatica
```

## Recursos principais

- Cadastro e consulta de jogos.
- Criacao e alteracao de status de pedidos.
- Integracao com Keycloak para login, refresh token e gestao de usuarios.
- Controle de usuarios com roles do Keycloak.
- Consulta GraphQL para jogos com filtros e ordenacao.
- Auditoria em MongoDB.
- Cache distribuido com Redis quando configurado.
- Tratamento centralizado de excecoes e respostas de validacao.

## Pre-requisitos

- .NET SDK 8.0
- Docker e Docker Compose
- Git

Opcional:

- Visual Studio 2022 ou VS Code
- Ferramenta `dotnet-ef`, caso queira executar comandos de migration manualmente

```bash
dotnet tool install --global dotnet-ef
```

## Como executar localmente

### 1. Subir as dependencias

Na raiz do projeto, execute:

```bash
docker compose up -d
```

Esse comando sobe:

- SQL Server em `localhost:1433`
- MongoDB em `localhost:27017`
- PostgreSQL do Keycloak em `localhost:5432`
- Keycloak em `http://localhost:8080`
- Redis em `localhost:6379`

O realm do Keycloak e importado automaticamente a partir de `docker/keycloak/realm-export.json`.

Credenciais administrativas do Keycloak:

```text
Usuario: admin
Senha: admin123
```

### 2. Restaurar dependencias .NET

```bash
dotnet restore TechChallengeFase1.slnx
```

### 3. Aplicar migrations no SQL Server

```bash
dotnet ef database update --project src/TechChallengeFase1.Infrastructure --startup-project src/TechChallengeFase1.Api
```

### 4. Executar a API

```bash
dotnet run --project src/TechChallengeFase1.Api
```

Por padrao, a API fica disponivel em:

- HTTP: `http://localhost:5066`
- HTTPS: `https://localhost:7272`
- Swagger: `http://localhost:5066/swagger`
- GraphQL: `http://localhost:5066/graphql`

## Configuracoes

As principais configuracoes estao em `src/TechChallengeFase1.Api/appsettings.json`.

```json
{
  "Keycloak": {
    "Authority": "http://localhost:8080/realms/TechChallengeFiap",
    "Audience": "tech-challenge",
    "BaseUrl": "http://localhost:8080/",
    "Realm": "TechChallengeFiap",
    "ClientId": "tech-challenge"
  },
  "MongoDb": {
    "ConnectionString": "mongodb://admin:PosTech@123@localhost:27017/?authSource=admin",
    "DatabaseName": "GamesLibraryAudit",
    "CollectionName": "AuditLogs"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=GamesLibrary;User Id=sa;Password=PosTech@123;TrustServerCertificate=True"
  }
}
```

Para usar Redis como cache distribuido, configure tambem:

```json
{
  "ConnectionStrings": {
    "Redis": "localhost:6379"
  }
}
```

Sem essa connection string, a aplicacao usa cache em memoria.

## Endpoints REST

### Autenticacao

| Metodo | Rota | Descricao |
| --- | --- | --- |
| POST | `/api/Auth/login` | Autentica usuario e retorna token JWT |
| POST | `/api/Auth/refresh` | Renova token usando refresh token |

### Usuarios

As rotas de usuarios exigem autenticacao com role `Admin`.

| Metodo | Rota | Descricao |
| --- | --- | --- |
| GET | `/api/Users` | Lista usuarios do Keycloak |
| POST | `/api/Users` | Cria usuario no Keycloak |
| PUT | `/api/Users/{userEmail}` | Atualiza usuario |
| PUT | `/api/Users/enable?userEmail={email}&enable=true` | Habilita ou desabilita usuario |
| GET | `/api/Users/search?username={username}&email={email}` | Busca usuario por username ou email |

### Jogos

| Metodo | Rota | Descricao |
| --- | --- | --- |
| POST | `/api/Game` | Cria um novo jogo |

### Pedidos

| Metodo | Rota | Descricao |
| --- | --- | --- |
| POST | `/api/Order` | Cria uma nova ordem |
| GET | `/api/Order/{id}` | Busca ordem por ID |
| PATCH | `/api/Order/{id}/status-change` | Altera status da ordem |

## GraphQL

O endpoint GraphQL fica em:

```text
http://localhost:5066/graphql
```

Exemplo de query:

```graphql
query {
  games {
    gameId
    description
    basePrice
  }
}
```

Tambem e possivel buscar um jogo por ID:

```graphql
query {
  gameById(gameId: 1) {
    gameId
    description
    basePrice
  }
}
```

## Testes

Para executar os testes automatizados:

```bash
dotnet test TechChallengeFase1.slnx
```

Os testes usam xUnit, Moq e EF Core InMemory.

## Comandos uteis

Subir containers:

```bash
docker compose up -d
```

Parar containers:

```bash
docker compose down
```

Aplicar migrations:

```bash
dotnet ef database update --project src/TechChallengeFase1.Infrastructure --startup-project src/TechChallengeFase1.Api
```

Rodar a API:

```bash
dotnet run --project src/TechChallengeFase1.Api
```

Rodar testes:

```bash
dotnet test TechChallengeFase1.slnx
```

## Observacoes

- O Keycloak importa o realm `TechChallengeFiap` automaticamente ao subir o container.
- O client configurado para a API e `tech-challenge`.
- A documentacao interativa da API fica disponivel via Swagger.
- O MongoDB e usado para registrar logs de auditoria.
- O SQL Server armazena os dados principais da aplicacao.
