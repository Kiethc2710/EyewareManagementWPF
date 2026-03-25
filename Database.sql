CREATE DATABASE EyewareManagement
GO

USE EyewareManagement
GO

CREATE TABLE Account (
AccountID INT PRIMARY KEY IDENTITY,
Username NVARCHAR(50) UNIQUE NOT NULL,
Password NVARCHAR(100) NOT NULL,
FullName NVARCHAR(100)
)

CREATE TABLE Customer (
CustomerID INT PRIMARY KEY IDENTITY,
FullName NVARCHAR(100) NOT NULL,
Phone NVARCHAR(15) NOT NULL,
Address NVARCHAR(200)
)

CREATE TABLE Category (
CategoryID INT PRIMARY KEY IDENTITY,
CategoryName NVARCHAR(100)
)

CREATE TABLE Product (
ProductID INT PRIMARY KEY IDENTITY,
ProductName NVARCHAR(100),
Brand NVARCHAR(100),
Price DECIMAL(10,2),
Quantity INT,
CategoryID INT,
FOREIGN KEY (CategoryID) REFERENCES Category(CategoryID)
)

CREATE TABLE Invoice (
InvoiceID INT PRIMARY KEY IDENTITY,
CreatedDate DATETIME DEFAULT GETDATE(),
CustomerID INT NOT NULL,
AccountID INT NOT NULL,
TotalAmount DECIMAL(12,2) DEFAULT 0,
FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID),
FOREIGN KEY (AccountID) REFERENCES Account(AccountID)
)

CREATE TABLE InvoiceDetail (
InvoiceDetailID INT PRIMARY KEY IDENTITY,
InvoiceID INT,
ProductID INT,
Quantity INT CHECK (Quantity > 0),
Price DECIMAL(10,2),
FOREIGN KEY (InvoiceID) REFERENCES Invoice(InvoiceID),
FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
)
-- =========================
-- ACCOUNT
-- =========================
INSERT INTO Account (Username, Password, FullName) VALUES
('staff1','123',N'Nguyễn Văn An'),
('staff2','123',N'Trần Thị Bình'),
('staff3','123',N'Lê Văn Cường')

-- =========================
-- CUSTOMER
-- =========================
INSERT INTO Customer (FullName, Phone, Address) VALUES
(N'Nguyễn Văn A','0901111111',N'HCM'),
(N'Trần Văn B','0902222222',N'Hà Nội'),
(N'Lê Thị C','0903333333',N'Đà Nẵng'),
(N'Phạm Văn D','0904444444',N'Cần Thơ')

-- =========================
-- CATEGORY
-- =========================
INSERT INTO Category (CategoryName) VALUES
(N'Kính cận'),
(N'Kính mát'),
(N'Kính thời trang')

-- =========================
-- PRODUCT
-- =========================
INSERT INTO Product (ProductName, Brand, Price, Quantity, CategoryID) VALUES
(N'Kính Rayban RB2140','Rayban',1500000,15,2),
(N'Kính Gucci GG001','Gucci',3200000,5,3),
(N'Kính cận Basic','NoBrand',500000,30,1),
(N'Kính Dior D100','Dior',2800000,8,3),
(N'Kính mát Oakley','Oakley',2000000,10,2)

-- =========================
-- INVOICE
-- =========================
INSERT INTO Invoice (CustomerID, AccountID, TotalAmount) VALUES
(1,1,0),
(2,2,0)

-- =========================
-- INVOICE DETAIL
-- =========================
INSERT INTO InvoiceDetail (InvoiceID, ProductID, Quantity, Price) VALUES
(1,1,1,1500000),
(1,3,2,500000),
(2,2,1,3200000),
(2,5,1,2000000)

-- =========================
-- UPDATE TOTAL AMOUNT
-- =========================
UPDATE Invoice
SET TotalAmount = (
SELECT SUM(Quantity * Price)
FROM InvoiceDetail
WHERE InvoiceDetail.InvoiceID = Invoice.InvoiceID
)

