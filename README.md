# 📘 Anticipation API

API para gerenciamento de solicitações de antecipação de valores.  
Projeto desenvolvido com **ASP.NET Core**, **Entity Framework Core** e arquitetura **hexagonal light**.

## ⚙️ Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- SQLite (Recomendado uso do SQLite Browser. Usar arquivo .db para visualizar a tabela.
- Ferramenta de linha de comando `dotnet-ef` para migrations:
  ```bash
  dotnet tool install --global dotnet-ef
  
## 🗄️ Criando Banco de Dados

```
dotnet ef migrations add InitialCreate -p LastLink.Anticipation.Infrastructure -s LastLink.Anticipation.Api
dotnet ef database update -p LastLink.Anticipation.Infrastructure -s LastLink.Anticipation.Api
```

## 📦 Como Executar Localmente

```
dotnet run --project LastLink.Anticipation.Api
Swagger irá abrir em uma aba do navegador automaticamente.
```
---

## 💰 Antecipação

### ▶️ POST `/api/v1/Anticipation`

Solicitar uma antecipação.

#### 🧾 Exemplo de Requisição

```json
{
  "creatorId": "11111111-1111-1111-1111-111111111111",
  "requestedAmount": 1500.00,
  "requestedAt": "2025-11-30T22:45:00"
}
```

#### ✅ Exemplo de Resposta

```json
{
  "id": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
  "creatorId": "11111111-1111-1111-1111-111111111111",
  "requestedAmount": 1500.00,
  "netAmount": 1425.00,
  "requestedAt": "2025-11-30T22:45:00",
  "status": "Pending"
}
```

---

### 🔍 GET `/api/v1/Anticipation/by-creator/{creatorId}`

Listar solicitações por criador.

#### 🧾 Exemplo de Requisição

```
Informar no input do Swagger creatorId.
```

#### ✅ Exemplo de Resposta

```json
[
  {
    "id": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
    "creatorId": "11111111-1111-1111-1111-111111111111",
    "requestedAmount": 1500.00,
    "netAmount": 1425.00,
    "requestedAt": "2025-11-30T22:45:00",
    "status": "Pending"
  }
]
```
---

### 🔍 GET `/api/v1/Anticipation/{id}`

Buscar solicitação por ID.

#### 🧾 Exemplo de Requisição

```
Informar no Swagger Id da solicitação que deseja pesquisar.
```

#### ✅ Exemplo de Resposta

```json
{
  "id": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
  "creatorId": "11111111-1111-1111-1111-111111111111",
  "requestedAmount": 1500.00,
  "netAmount": 1425.00,
  "requestedAt": "2025-11-30T22:45:00",
  "status": "Pending"
}

```
---

### ✏️ PATCH `/api/v1/Anticipation/{id}/approve`

Aprovar solicitação
#### 🧾 Exemplo de Requisição

```json
Informar no input do Swagger o ID da solicitação que deseja aprovar.
```

#### ✅ Exemplo de Resposta

```json
{
  "id": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
  "creatorId": "11111111-1111-1111-1111-111111111111",
  "requestedAmount": 1500.00,
  "netAmount": 1425.00,
  "requestedAt": "2025-11-30T22:45:00",
  "status": "Approved"
}
```
---

### ✏️ PATCH `/api/v1/Anticipation/{id}/reject`

Rejeitar solicitação.

#### 🧾 Exemplo de Requisição

```
Informar no input do Swagger ID da antecipação que deseja rejeitar.
```

#### ✅ Exemplo de Resposta

```json
{
  "id": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
  "creatorId": "11111111-1111-1111-1111-111111111111",
  "requestedAmount": 1500.00,
  "netAmount": 1425.00,
  "requestedAt": "2025-11-30T22:45:00",
  "status": "Rejected"
}
```

---

## ❌ Exemplos de erros de validação

### ▶️ POST `/api/v1/Anticipation`

Criar solicitação com valor abaixo do permitido (100,00)

#### 🧾 Exemplo de Requisição

```json
{
  "creatorId": "11111111-1111-1111-1111-111111111111",
  "requestedAmount": 50,
  "requestedAt": "2025-11-30T22:45:00"
}
```

#### ❌ Exemplo de Resposta

```{
  "errors": {
    "RequestedAmount": [
      "Requested amount must be greater than 100."
    ]
  }
}
```
---

Criar solicitação com requestedAt no futuro.

#### 🧾 Exemplo de Requisição

```json
{
  "creatorId": "11111111-1111-1111-1111-111111111111",
  "requestedAmount": 500,
  "requestedAt": "2026-01-01T10:00:00"
}
```

#### ❌ Exemplo de Resposta

```{
  "errors": {
    "RequestedAt": [
      "RequestedAt cannot be in the future."
    ]
  }
}
```
