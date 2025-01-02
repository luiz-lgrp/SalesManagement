-- Criação do banco de dados com o nome correto
CREATE DATABASE SalesManagementDb;
GO

-- Usar o banco criado
USE SalesManagementDb;
GO

-- Criação das tabelas
CREATE TABLE Customers (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Name NVARCHAR(100),
    Cpf NVARCHAR(20),
    Email NVARCHAR(100),
    Phone NVARCHAR(20),
    Status BIT,
    Created DATETIME,
    Updated DATETIME
);

CREATE TABLE Products (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    ProductName NVARCHAR(100),
    Stock INT,
    Price DECIMAL(18,2),
    Status BIT,
    Created DATETIME,
    Updated DATETIME
);

CREATE TABLE Orders (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Cpf NVARCHAR(20),
    OrderCode NVARCHAR(20),
    TotalValue DECIMAL(18,2),
    Status BIT,
    Created DATETIME,
    Updated DATETIME
);

CREATE TABLE OrderItems (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    ProductId UNIQUEIDENTIFIER,
    OrderCode NVARCHAR(20),
    ProductName NVARCHAR(100),
    Quantity INT,
    UnitValue DECIMAL(18,2),
    OrderId UNIQUEIDENTIFIER,
    Created DATETIME,
    Updated DATETIME
);
