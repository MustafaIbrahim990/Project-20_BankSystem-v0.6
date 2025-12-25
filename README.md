# 🏦 Project-20: BankSystem (v0.6) — Core Banking API

Author: Mustafa A. Ibrahim

This repository contains a modern, scalable, and secure Core Banking Backend System built using ASP.NET Core Web API (.NET 8) and designed according to Clean Architecture principles.

Version v0.6 represents a major architectural refactor, transforming the system from a legacy desktop-based solution into a fully decoupled, asynchronous Web API capable of serving:
- 🌐 Web applications  
- 📱 Mobile applications  
- 🔌 Third-party services  

All data is persistently stored in SQL Server, ensuring data integrity, consistency, and scalability.

---

## 📅 General Overview & Functionality

- Manage clients and bank accounts  
- Perform secure financial transactions (Deposit, Withdraw, Transfer)  
- User authentication with role-based authorization  
- Secure PIN handling using hashing & salting  
- Currency & country management  
- Auditing and logging of sensitive operations  
- Reliable and transactional data persistence using SQL Server  

---

## 🚀 Core Features

- 👥 Client Management
  - Full CRUD operations
  - Account creation and retrieval

- 💸 Transaction Processing
  - Deposits
  - Withdrawals
  - Transfers
  - Atomic and secure SQL transactions

- 🔐 Security
  - User authentication
  - Role-based authorization
  - SHA256 hashing + salting for PINs

- 🧮 Data Integrity
  - Explicit SQL Transactions
  - Row-level locking to prevent race conditions

- 🌍 Global Data Management
  - Countries
  - Currencies
  - Exchange rates

- 📝 Auditing & Logging
  - User login logs
  - Client transfer history

---

## 🧱 Architecture

BankSystem v0.6 is built using a Layered Architecture aligned with Clean Architecture concepts.

🔁 Dependency Flow Rule  
> Outer layers depend on inner layers — never the opposite.

### 📂 Project Layers

- **API Layer (BankSystem.API)**
  - ASP.NET Core Web API
  - Routing, controllers, request/response handling
  - Error mapping

- **Business Logic Layer (BankSystem.BLL)**
  - Business rules
  - Validation logic
  - Security (hashing & salting)
  - Orchestration of operations

- **Data Access Layer (BankSystem.DAL)**
  - Direct SQL Server access
  - ADO.NET (asynchronous)
  - Stored Procedures execution

- **DTOs Layer (BankSystem.DTOs)**
  - Data Transfer Objects
  - Communication contracts between layers

- **Domain Layer (BankSystem.Domain)**
  - Core entities and value objects
  - Pure business models with no external dependencies

---

## 💻 TLanguage:sed

- *Framework:#  
- **Framework:** ASP.NET Core WebDatabase:0)  
- **Database:** SQLData Access: 
- **Data Access:** Security:c)  
- **Security:** SHA256 HaTransactions:
- **Transactions:** SQL Transactions  

---

## 🔧 Key Highlights

- ✅ Clean & Layered Architecture  
- 🔄 Fully asynchronous data access  
- 🔐 High-security standards for sensitive data  
- 🧱 Strong separation of concerns  
- 📦 DTO-based communication  
- 🧪 Ready for future unit & integration testing  
- 🌱 Scalable foundation for future UI clients  

---

## ⬇️ Installation & Running

### 🔧 Prereq8.0

- .NET SDK **8.0** or later  
- SQL Server (Express / Developer / Full Edition)  
- SQL Server Management Studio (SSMS)

---

### ⚙️ Database Setup

1. Execute the database script:
