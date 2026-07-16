# 💰 Finance Flow

<div align="center">

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-17-336791?style=for-the-badge&logo=postgresql&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2CA5E0?style=for-the-badge&logo=docker&logoColor=white)
![Tests](https://img.shields.io/badge/Tests-xUnit-512BD4?style=for-the-badge)
![CI/CD](https://img.shields.io/badge/CI%2FCD-GitHub%20Actions-2088FF?style=for-the-badge&logo=github-actions)
![License](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)
[![codecov](https://codecov.io/gh/antoniomrrds/finance_flow/branch/main/graph/badge.svg?token=orNk9n8LG6)](https://codecov.io/gh/antoniomrrds/finance_flow)

</div>

## 📖 Sobre o Projeto

**Finance Flow** é uma API robusta para gerenciamento de fluxo de caixa pessoal desenvolvida com **ASP.NET Core 10.0** e **PostgreSQL 17**.

Desenvolvida com foco em boas práticas de software, arquitetura limpa e testes abrangentes.

## ✨ Principais Características

- 🏗️ **Arquitetura Limpa** com separação de responsabilidades
- 🔄 **Versionamento de API** com ASP.NET Core API Versioning
- ✅ **Validação Robusta** com FluentValidation em português
- 📊 **Documentação Automática** com Swagger/OpenAPI
- 💾 **PostgreSQL 17** como banco de dados principal
- 🧪 **Testes Abrangentes** (unitários, integração e E2E)
- 🚀 **CI/CD** com GitHub Actions
- 🐳 **Docker & Docker Compose** pré-configurado
- 🔐 **Health Checks** integrados
- 🐺 **Git Hooks** com Husky.NET

## 🚀 Tecnologias Utilizadas

### Core
- .NET 10.0
- C# 13
- ASP.NET Core 10.0
- PostgreSQL 17
- Entity Framework Core 10.0

### Arquitetura & Padrões
- ASP.NET Core API Versioning
- FluentValidation
- Scrutor (auto-registration de dependências)
- EFCore.NamingConventions

### Testes
- xUnit v3
- NSubstitute
- Shouldly
- Bogus
- WebApplicationFactory

### DevOps
- Docker & Docker Compose
- GitHub Actions
- Coverlet

## 📋 Pré-requisitos

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker](https://www.docker.com/)
- [PostgreSQL 17](https://www.postgresql.org/) (ou use Docker)

## 🛠️ Como Começar

### 1. Clone o Repositório

```bash
git clone https://github.com/antoniomrrds/finance_flow.git
cd finance_flow
```

### 2. Configure as Variáveis de Ambiente

Copie o arquivo `.env.example` e renomeie para `.env`:

```bash
cp .env.example .env
```

Edite o `.env` com suas configurações:

```env
POSTGRES_DB=finance_flow_db
POSTGRES_USER=finance_flow_user
POSTGRES_PASSWORD=seu_senha_segura
```

### 3. Inicie o PostgreSQL com Docker

```bash
docker compose up -d
```

### 4. Aplique as Migrações

```bash
cd src/WebApi
dotnet ef database update
cd ../..
```

### 5. Execute a Aplicação

```bash
dotnet run --project src/WebApi/WebApi.csproj
```

A API estará disponível em:
- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`

## 📚 Documentação da API

Após iniciar a aplicação, acesse:

- **Swagger UI**: `http://localhost:5000/swagger/index.html`
- **OpenAPI JSON**: `http://localhost:5000/openapi/v1.json`
- **Health Check**: `http://localhost:5000/health`

## 🔗 Endpoints Principais

### Categorias

| Método | Endpoint | Descrição |
|--------|----------|----------|
| GET | `/api/v1/categories` | Listar todas as categorias |
| GET | `/api/v1/categories/{id}` | Obter categoria por ID |
| POST | `/api/v1/categories` | Criar nova categoria |
| PUT | `/api/v1/categories/{id}` | Atualizar categoria |
| DELETE | `/api/v1/categories/{id}` | Deletar categoria |

### Despesas

| Método | Endpoint | Descrição |
|--------|----------|----------|
| GET | `/api/v1/expenses` | Listar todas as despesas |
| GET | `/api/v1/expenses/{id}` | Obter despesa por ID |
| POST | `/api/v1/expenses` | Criar nova despesa |
| PUT | `/api/v1/expenses/{id}` | Atualizar despesa |
| DELETE | `/api/v1/expenses/{id}` | Deletar despesa |

## 📊 Exemplos de Uso

### Criar Categoria

```bash
curl -X POST http://localhost:5000/api/v1/categories \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Alimentação",
    "description": "Gastos com comida"
  }'
```

### Criar Despesa

```bash
curl -X POST http://localhost:5000/api/v1/expenses \
  -H "Content-Type: application/json" \
  -d '{
    "description": "Almoço",
    "value": 45.90,
    "date": "2026-01-15T12:00:00Z",
    "categoryId": 1
  }'
```

### Listar Despesas

```bash
curl http://localhost:5000/api/v1/expenses
```

## 🧪 Testes

### Executar Todos os Testes

```bash
dotnet test FinanceFlow.slnx
```

### Testes por Categoria

```bash
# Apenas testes unitários
dotnet test FinanceFlow.slnx --filter "Category=Unit"

# Apenas testes de integração
dotnet test FinanceFlow.slnx --filter "Category=Integration"

# Apenas testes E2E
dotnet test FinanceFlow.slnx --filter "Category=E2E"
```

### Cobertura de Código

```bash
dotnet test FinanceFlow.slnx --collect:"XPlat Code Coverage"
```

## 🔧 Comandos Úteis

### Desenvolvimento

```bash
# Restaurar dependências
dotnet restore FinanceFlow.slnx

# Build
dotnet build FinanceFlow.slnx

# Build Release
dotnet build FinanceFlow.slnx --configuration Release

# Watch mode (hot reload)
dotnet watch --project src/WebApi/WebApi.csproj run
```

### Entity Framework

```bash
cd src/WebApi

# Criar migração
dotnet ef migrations add NomeDaMigracao

# Aplicar migrações
dotnet ef database update

# Remover última migração
dotnet ef migrations remove

# Listar migrações
dotnet ef migrations list
```

### Docker

```bash
# Iniciar containers
docker compose up -d

# Parar containers
docker compose down

# Ver logs
docker compose logs -f postgres

# Reset completo (remove volumes)
docker compose down -v
```

## 📁 Estrutura do Projeto

```
finance_flow/
├── src/
│   ├── WebApi/                 # Projeto principal da API
│   │   ├── Program.cs
│   │   ├── appsettings.json
│   │   ├── appsettings.Development.json
│   │   ├── Configuration/      # Configuração da aplicação
│   │   ├── Domain/             # Entidades de negócio
│   │   ├── Features/           # Features da aplicação
│   │   ├── Infrastructure/     # Camada de infraestrutura
│   │   ├── Shared/             # Código compartilhado
│   │   └── WebApi.csproj
│   │
│   ├── SharedKernel/           # Kernel compartilhado
│   └── SrcSharedPackages.props
│
├── tests/
│   ├── WebApi.Tests/           # Testes automatizados
│   ├── TestUtilities/          # Utilitários para testes
│   └── TestSharedPackages.props
│
├── .github/
│   └── workflows/
│       └── ci.yml              # Pipeline CI/CD
│
├── .husky/                     # Git hooks
├── .vscode/                    # Configurações VS Code
│
├── docker-compose.yml          # Orquestração Docker
├── Directory.Build.props       # Propriedades de build
├── Directory.Packages.props    # Gerenciamento de pacotes
├── FinanceFlow.slnx            # Solution do projeto
├── .env.example                # Exemplo de variáveis de ambiente
├── .editorconfig               # Configurações do editor
└── README.md                   # Este arquivo
```

## 🔐 Configuração de Ambiente

### Arquivo `.env`

```env
POSTGRES_DB=finance_flow_db
POSTGRES_USER=finance_flow_user
POSTGRES_PASSWORD=sua_senha_aqui
```

### Arquivo `appsettings.Development.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=5432;Database=finance_flow_db;User Id=finance_flow_user;Password=sua_senha_aqui;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  }
}
```

## 🚀 CI/CD - GitHub Actions

O projeto possui um pipeline automatizado que executa em cada push/PR para `main`:

**Arquivo**: `.github/workflows/ci.yml`

**O que faz**:
1. ✅ Build da solução
2. 🧪 Executa todos os testes
3. 📈 Coleta cobertura de código
4. 📤 Envia cobertura para Codecov (se configurado)

## 🐺 Git Hooks

O projeto utiliza **Husky.NET** para automatizar verificações:

- **pre-commit**: Formata o código
- **commit-msg**: Valida mensagens (Conventional Commits)
- **pre-push**: Executa os testes

## 📝 Padrões de Commit

O projeto segue **[Conventional Commits](https://www.conventionalcommits.org/pt-br/)**:

```bash
feat:    Nova funcionalidade
fix:     Correção de bug
docs:    Documentação
style:   Formatação
refactor: Refatoração
test:    Testes
chore:   Configurações e dependências
perf:    Performance
```

**Exemplos**:
```bash
git commit -m "feat: adicionar validação de categorias"
git commit -m "fix: corrigir cálculo de total de despesas"
git commit -m "docs: atualizar documentação da API"
```

## 🤝 Contribuindo

1. Fork o projeto
2. Crie uma branch (`git checkout -b feature/minha-feature`)
3. Faça commit (`git commit -m 'feat: descrição'`)
4. Adicione testes
5. Execute os testes (`dotnet test`)
6. Push (`git push origin feature/minha-feature`)
7. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT. Veja [LICENSE](LICENSE) para mais detalhes.

## 👨‍💻 Autor

**Antonio Tech**
- 🐙 GitHub: [@antoniomrrds](https://github.com/antoniomrrds)

## 📞 Suporte

- 🐛 [Reportar Bug](https://github.com/antoniomrrds/finance_flow/issues/new?template=bug_report.md)
- 💡 [Sugerir Feature](https://github.com/antoniomrrds/finance_flow/issues/new?template=feature_request.md)

---

<div align="center">

**⭐ Se este projeto te ajudou, considere dar uma estrela!**

Feito com ❤️ por [Antonio Tech](https://github.com/antoniomrrds)

</div>
