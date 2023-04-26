Create DATABASE Inventory_Management;
use Inventory_Management;
Create Table Users(
	UserId varchar(100)  PRIMARY KEY,
	Firstname varchar(100) NOT NULL,
	Lastname varchar(100) NOT NULL,
	Address varchar(100) NOT NULL,
	Username varchar(100) NOT NULL,
	PasswordHash varchar(100) NOT NULL,
	Mail varchar(100) NOT NULL,
	Phone varchar(11) NOT NULL,
	Age integer NOT NULL,
	isAdmin integer NOT NULL
);
Create Table Products(
	ProductId varchar(100) NOT NULL  PRIMARY KEY,
	Name varchar(100) NOT NULL,
	Size char NOT NULL,
	Price integer NOT NULL,
	Quantity integer NOT NULL
);

Create Table Orders(
	OrderId varchar(100) NOT NULL PRIMARY KEY,
	ProductId varchar(100) NOT NULL,
	CustomerId varchar(100) NOT NULL,
	Date DateTime NOT NULL,
    FOREIGN KEY (CustomerId) REFERENCES Users(UserId) ON DELETE CASCADE
);