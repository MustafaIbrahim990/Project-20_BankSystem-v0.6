# 🏦 Project-20: BankSystem (v0.6) — Core Banking API

Author: Mustafa A. Ibrahim  

---

## 📌 Overview

This repository contains a modern, scalable, and secure Core Banking Backend System built using ASP.NET Core Web API (.NET 8) and designed according to Clean Architecture principles.

🔄 Version v0.6 represents a major architectural refactor, transforming the system from a legacy desktop-based solution into a fully decoupled, asynchronous Web API.

The API is capable of integrating with any client technology that consumes RESTful APIs, including:

- 🌐 Web Frontend Frameworks (React, Angular, Vue, Blazor)
- 📱 Mobile Applications (Flutter, React Native)
- 🖥 Desktop Applications
- 🔌 Third-party & Enterprise Systems

🗄 All data is persistently stored in SQL Server, ensuring data integrity, consistency, and scalability.

---

## 📅 General Functionality

- 👤 Client & Bank Account Management  
- 💸 Secure Financial Transactions (Deposit, Withdraw, Transfer)  
- 🔐 Secure PIN handling (Hashing & Salting)  
- 🌍 Currency & Country Management  
- 📝 Auditing & Logging of sensitive operations  
- 🧮 Reliable transactional data persistence using SQL Server  

---

## 🚀 Core Features

### 👥 Client Management
- Full CRUD operations  
- Account creation & retrieval  

### 💸 Transaction Processing
- Deposits  
- Withdrawals  
- Transfers  
- Atomic & secure SQL transactions  

### 🔐 Security
- SHA256 hashing with salting for PINs  

### 🧮 Data Integrity
- Explicit SQL Transactions  
- Row-level locking to prevent race conditions  

### 🌍 Global Data Management
- Countries  
- Currencies  
- Exchange Rates  

### 📝 Auditing & Logging
- User login logs  
- Client transfer history  

---

## 🧱 Architecture

BankSystem v0.6 follows a Layered Architecture aligned with Clean Architecture concepts.

🔁 Dependency Flow Rule  
> Outer layers depend on inner layers — never the opposite.

### 📂 Project Layers

- API Layer (BankSystem.API)  
  - ASP.NET Core Web API  
  - Routing & Controllers  
  - Request/Response handling  
  - Error mapping  

- Business Logic Layer (BankSystem.BLL)  
  - Business rules  
  - Validation logic  
  - Security (Hashing & Salting)  
  - Operation orchestration  

- Data Access Layer (BankSystem.DAL)  
  - Direct SQL Server access  
  - Asynchronous ADO.NET  
  - Stored Procedures execution  

- DTOs Layer (BankSystem.DTOs)  
  - Data Transfer Objects  
  - Communication contracts between layers  

- Domain Layer (BankSystem.Domain)  
  - Core entities & value objects  
  - Pure business models (no external dependencies)  

---

## 💻 Technology Stack

- Language: C#  
- Framework: ASP.NET Core Web API (.NET 8)  
- Database: SQL Server  
- Data Access: ADO.NET (Async)  
- Security: SHA256 Hashing + Salting  
- Transactions: SQL Transactions  

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

### 🔧 Prerequisites
- .NET SDK 8.0 or later  
- SQL Server (Express / Developer / Full Edition)  
- SQL Server Management Studio (SSMS)  

---

### ⚙️ Database Setup

1. Execute the provided database script.  
   This will create:
   - Database  
   - Tables  
   - Stored Procedures  
   - Initial seed data  

2. Update the connection string in:
BankSystem.DAL/Settings/DataSettings

---

## ▶️ Running the API

1. Clone the repository:

`bash
git clone https://github.com/MustafaIbrahim990/Project-20_BankSystem-v0.6.git
