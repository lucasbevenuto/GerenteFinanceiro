# Gerente Financeiro API

API para gerenciamento financeiro pessoal desenvolvida em C# com ASP.NET Core e organizada com princípios de Clean Architecture.

O projeto permite cadastrar usuários, categorias e transações, gerar relatório financeiro mensal e consultar cotação de moedas em tempo real por integração externa. Além da API, a aplicação também possui uma interface web simples para testes locais no navegador.

## Visão geral

O objetivo do projeto é consolidar conhecimentos em:

- desenvolvimento backend com .NET
- modelagem de dados
- consumo de APIs externas
- separação de responsabilidades em camadas
- construção de uma base escalável para evolução futura

## Funcionalidades

- Cadastro de usuários
- Cadastro de categorias de receita e despesa
- Registro de transações financeiras
- Consulta de transações com filtros
- Relatório financeiro mensal por usuário
- Integração com API externa de cotação de moedas
- Interface web local para validar o funcionamento da API
- Tratamento global de erros com respostas padronizadas

## Tecnologias utilizadas

- C#
- .NET 9
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server LocalDB
- xUnit
- REST API

## Arquitetura

O projeto segue uma estrutura inspirada em Clean Architecture:

- `FinanceManager.Domain`
  Regras de negócio, entidades e enums.
- `FinanceManager.Application`
  Casos de uso, DTOs, contratos, validações e serviços da aplicação.
- `FinanceManager.Infrastructure`
  Persistência com Entity Framework Core, repositórios, segurança e integrações externas.
- `FinanceManager.API`
  Exposição dos endpoints HTTP e interface web local.
- `FinanceManager.Tests`
  Testes automatizados das regras principais.

## Estrutura da solução

```text
FinanceManager
├── FinanceManager.API
├── FinanceManager.Application
├── FinanceManager.Domain
├── FinanceManager.Infrastructure
└── FinanceManager.Tests
```

## Pré-requisitos

Antes de executar o projeto, garanta que sua máquina tenha:

- .NET SDK 9 instalado
- SQL Server LocalDB disponível

Observação:
O projeto está configurado por padrão para usar LocalDB com a connection string definida em `FinanceManager.API/appsettings.json`.

## Como executar o projeto

1. Clone o repositório:

```bash
git clone https://github.com/lucasbevenuto/GerenteFinanceiro.git
```

2. Acesse a pasta da solução:

```bash
cd GerenteFinanceiro/FinanceManager
```

3. Restaure os pacotes:

```bash
dotnet restore
```

4. Execute a API:

```bash
dotnet run --project ./FinanceManager.API
```

## Acesso local

Com a aplicação em execução, você pode acessar:

- Interface web local: `http://localhost:5221`
- API HTTP: `http://localhost:5221`
- API HTTPS: `https://localhost:7195`

Na primeira execução, a aplicação garante automaticamente a criação do banco com `EnsureCreated()`.

## Interface web

O projeto possui uma interface web simples embutida na própria API para facilitar os testes no navegador.

Pela interface é possível:

- criar usuários
- criar categorias
- criar transações
- listar registros
- gerar relatório mensal
- consultar cotação de moedas

Fluxo sugerido para validação:

1. Criar um usuário
2. Criar uma categoria de receita
3. Criar uma categoria de despesa
4. Criar uma ou mais transações
5. Gerar o relatório mensal
6. Consultar a cotação entre moedas

## Principais endpoints

### Usuários

- `GET /api/users`
- `GET /api/users/{id}`
- `POST /api/users`

### Categorias

- `GET /api/categories`
- `GET /api/categories/{id}`
- `POST /api/categories`

### Transações

- `GET /api/transactions`
- `GET /api/transactions/{id}`
- `POST /api/transactions`
- `PUT /api/transactions/{id}`
- `DELETE /api/transactions/{id}`

### Relatórios

- `GET /api/reports/monthly?userId={guid}&year={ano}&month={mes}`

### Cotação de moedas

- `GET /api/exchangerates/latest?baseCurrency=USD&targetCurrency=BRL&amount=10`

## Exemplos de payload

### Criar usuário

```json
{
  "name": "Maria Silva",
  "email": "maria@email.com",
  "password": "123456"
}
```

### Criar categoria

```json
{
  "name": "Salario",
  "type": 1
}
```

### Criar transação

```json
{
  "description": "Salario de abril",
  "amount": 3500.00,
  "date": "2026-04-07T00:00:00",
  "type": 1,
  "userId": "00000000-0000-0000-0000-000000000000",
  "categoryId": "00000000-0000-0000-0000-000000000000"
}
```

## Tipos de transação

O enum `TransactionType` é representado numericamente:

- `1` = `Receita`
- `2` = `Despesa`

## Testes

Para executar os testes automatizados:

```bash
dotnet test FinanceManager.sln
```

Os testes atuais cobrem cenários importantes como:

- criação de usuário
- rejeição de e-mail duplicado
- consistência entre tipo de transação e categoria
- cálculo do relatório mensal

## Tratamento de erros

A API possui tratamento global de exceções e responde com `ProblemDetails`.

Exemplos de erros tratados:

- recurso não encontrado
- falhas de validação
- erros internos inesperados

## Integração externa

A consulta de cotação de moedas utiliza a API Frankfurter, configurada em:

- `FinanceManager.API/appsettings.json`

Você pode alterar a URL base da integração pela chave:

```json
"ExchangeRates": {
  "BaseUrl": "https://api.frankfurter.dev/v1/"
}
```

## Próximos passos sugeridos

- adicionar autenticação e autorização
- criar migrações do Entity Framework Core
- ampliar cobertura de testes
- adicionar paginação e filtros mais avançados
- evoluir a interface web

## Autor

Projeto mantido por Lucas Bevenuto.
