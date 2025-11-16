# 🏭 InventoryManagementAPI

API REST desenvolvida em **C# (.NET 8)** para gerenciamento de inventário, incluindo autenticação de usuários e CRUD completo de produtos.  
A aplicação utiliza **Oracle Database** e segue boas práticas de organização com **Controllers**, **Services**, **Repositories** e **DTOs**.

---

## 🚀 Tecnologias Utilizadas
- C# / .NET 8  
- ASP.NET Core Web API  
- Oracle Database  
- Oracle.ManagedDataAccess.Core  
- DTOs  
- Injeção de Dependência  
- Arquitetura em Camadas  

---

## 🛢 Estrutura do Banco (Oracle)

### USERS
| Campo         | Tipo        |
|--------------|-------------|
| ID           | NUMBER (PK) |
| NAME         | VARCHAR2    |
| EMAIL        | VARCHAR2    |
| PASSWORD_HASH| VARCHAR2    |
| CREATED_AT   | TIMESTAMP   |
| UPDATED_AT   | TIMESTAMP   |

### PRODUCTS
| Campo       | Tipo        |
|-------------|-------------|
| ID          | NUMBER (PK) |
| NAME        | VARCHAR2    |
| DESCRIPTION | VARCHAR2    |
| PRICE       | NUMBER      |
| QUANTITY    | NUMBER      |
| CREATED_AT  | TIMESTAMP   |
| UPDATED_AT  | TIMESTAMP   |

---

## 🔌 Rotas da API

### 🔐 Autenticação
**POST** `/auth/login`  
**POST** `/auth/register`

### 📦 Produtos
**GET** `/products`  
**POST** `/products`  
**PUT** `/products/{id}`  
**DELETE** `/products/{id}`  

---

## ▶️ Como Executar

### 1. Configure a Connection String no `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "User Id=SYSTEM;Password=xxxx;Data Source=localhost:1521/XEPDB1"
}
```
### 2. Restaurar dependências:
```code
dotnet restore
```
### 3. Executar API:
```code
dotnet run
```
## API disponível em:
👉 http://localhost:5000
ou
👉 https://localhost:7000
