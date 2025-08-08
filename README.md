# StoreCard Backend API

A clean architecture-based backend solution for managing users and their store credit/debit transactions, built with .NET Core, Entity Framework Core, and RESTful API principles.

---

## Features

- **User Management**: CRUD operations for users.
- **Transactions**: Store credit and debit transaction handling per user.
- **Transaction Summaries**: Efficient in-memory strategies to calculate totals per user, transaction type, and high-volume transactions.
- **Clean Architecture**: Layered project structure ensuring separation of concerns.
- **Entity Framework Core**: SQL Server database support with in-memory provider for testing.
- **AutoMapper**: DTO mappings for clean API models.
- **Swagger**: API documentation and testing.
- **Logging**: Integrated logging for troubleshooting.
- **Unit & Integration Tests**: NUnit and NSubstitute for isolated unit tests; integration tests use an in-memory database.

---

## Getting Started

### Prerequisites

- [.NET 7 SDK or later](https://dotnet.microsoft.com/download)
- SQL Server (for production)

### Setup

1. **Clone the repository:**

   ```bash
   git clone https://github.com/Joisys/StoreCard.git
   cd storecard
