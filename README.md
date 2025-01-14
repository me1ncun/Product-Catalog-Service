# Product Catalog API

## Overview

This service is designed for managing a product catalog, enabling seamless CRUD (Create, Read, Update, Delete) operations for maintaining product records. Each product in the catalog includes essential properties, providing robust functionality for inventory management. The REST API ensures efficient interaction with the catalog, supporting both detailed data retrieval and streamlined updates.

## Tech Stack

- .NET 8.0: The application is built using ASP.NET Core, leveraging modern C# features and practices.
- PostgreSQL: powerful, open source object-relational database system.
- Entity Framework Core: ORM used for database interactions.
- AutoMapper: Used for mapping between entities and DTOs (Data Transfer Objects).
- FluentValidator: Used for validating objects with custom rules in readable syntax.
- Swagger: API documentation and testing tool.
- Postman: platform for collaborative API development.
- xUnit: Unit testing framework.

## Features

- Get All Products:  Retrieve a list of products. The method returns the following fields: product code, name, and price.
- Create Product: Allows users to add a new product to the catalog by specifying all necessary fields.
- Get Product By Code: Retrieve detailed information about a product using its unique product code. The response includes all fields available for the specified product.
- Edit Product: Update the information of an existing product in the catalog.
- Delete Product: Remove a product from the catalog entirely.

## Setup Instructions

### Locally

### 1. Clone the repository to your local machine

```cmd
git clone https://github.com/me1ncun/Product-Catalog-Service.git
```

### 2. Set Up the Database

- Modify the appsettings.json file to include your database connection string:

```json
 "ConnectionStrings": {
    "DatabaseConnectionString": "Host=localhost;Port=5432;Database=product-catalog-system;Username=postgres;Password=postgres;"
}
```

### 3. Migrate the Database

After setting up your connection string, the database will automatically update when the application starts. You can also run the following command manually to apply any pending migrations:

```cmd
dotnet ef database update
```

### 4. Run the Application

To run the API locally, use the following command:

```cmd
dotnet run
```

### 5. Testing the Endpoints

Once the application is running, you can test the endpoints using Swagger UI.

- Swagger UI: Navigate to https://localhost:7080/swagger/index.html for a visual interface to test the API.
- Postman: Navigate to https://www.postman.com/docking-module-architect-85396035/workspace/product-api for testing endpoints.

### 6. Tests

Unit tests for the controller are written using xUnit and Moq. These tests can be run with the following command:

```cmd
dotnet test
```
