---CREATE A SEPARATE SCHEMA for Exchange Rate FYP

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.SCHEMATA(NOLOCk) where SCHEMA_NAME = 'ExchangeRateFYP')
Begin
	CREATE SCHEMA ExchangeRateFYP
END

----CLEAN UP TABLES & RECORDS
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES(NolocK) where TABLE_SCHEMA = 'ExchangeRateFYP' and TABLE_NAME ='ExchangeRates')
BEGIN
 DROP TABLE ExchangeRateFYP.ExchangeRates
END

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES(NolocK) where TABLE_SCHEMA = 'ExchangeRateFYP' and TABLE_NAME ='Enquiries')
BEGIN
 DROP TABLE ExchangeRateFYP.Enquiries
END

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES(NolocK) where TABLE_SCHEMA = 'ExchangeRateFYP' and TABLE_NAME ='Transactions')
BEGIN
 DROP TABLE ExchangeRateFYP.Transactions
END

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES(NolocK) where TABLE_SCHEMA = 'ExchangeRateFYP' and TABLE_NAME ='Countries')
BEGIN
 DROP TABLE ExchangeRateFYP.Countries
END

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES(NolocK) where TABLE_SCHEMA = 'ExchangeRateFYP' and TABLE_NAME ='Users')
BEGIN
 DROP TABLE ExchangeRateFYP.Users
END

---CREATE TABLES REQUIRED 
--USER MODULES
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES(NolocK) where TABLE_SCHEMA = 'ExchangeRateFYP' and TABLE_NAME ='Users')
BEGIN
	CREATE TABLE [ExchangeRateFYP].Users (
		UserID INT IDENTITY PRIMARY KEY,
		UserType int null,
		Username nvarchar(50) null,
		Passwords nvarchar(50) null,
		EmailAddress nvarchar(50) null,
		Remarks nvarchar(50) null,
		IsActive bit,
		CreatedBy int,
		CreatedDate datetime,
		ModifiedBy int,
		ModifiedDate datetime
	)
END
---User Type = 1 (ADMIN), 2 (STAFF), 3 (Customer)
--=please login use admin to login
	INSERT INTO [ExchangeRateFYP].[Users]
           ([UserType]
           ,[Username]
           ,[Passwords]
           ,[EmailAddress]
           ,[Remarks]
           ,[IsActive]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
VALUES(1,'admin','admin','ADMIN@admin.com','This is admin account',1,1,GETDATE(),1,getdate())

--Country
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES(NolocK) where TABLE_SCHEMA = 'ExchangeRateFYP' and TABLE_NAME ='Countries')
BEGIN
	CREATE TABLE [ExchangeRateFYP].Countries (
		CountryID INT IDENTITY PRIMARY KEY,
		CountryName nvarchar(50) null,
		CurrentCurrencyRate decimal(10,2) null,
		Remarks nvarchar(50) null,
		IsActive bit,
		CreatedBy int,
		CreatedDate datetime,
		ModifiedBy int,
		ModifiedDate datetime
	)
END
--ExchangeRates
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES(NolocK) where TABLE_SCHEMA = 'ExchangeRateFYP' and TABLE_NAME ='ExchangeRates')
BEGIN
	CREATE TABLE [ExchangeRateFYP].ExchangeRates (
		ExchangeRateID INT IDENTITY PRIMARY KEY,
		FromCountryID int REFERENCES [ExchangeRateFYP].Countries(CountryID),
		FromCountryRate decimal(10,2) null, 
		ToCountryID int REFERENCES [ExchangeRateFYP].Countries(CountryID),
		ToCountryRate decimal(10,2) null,
		Remarks nvarchar(50) null,
		IsActive bit,
		CreatedBy int,
		CreatedDate datetime,
		ModifiedBy int,
		ModifiedDate datetime
	)
END
--Transactions
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES(NolocK) where TABLE_SCHEMA = 'ExchangeRateFYP' and TABLE_NAME ='Transactions')
BEGIN
	CREATE TABLE [ExchangeRateFYP].Transactions (
		TransactionID INT IDENTITY PRIMARY KEY,
		FromCountryID int REFERENCES [ExchangeRateFYP].Countries(CountryID),
		FromCountryRate decimal(10,2) null, 
		ToCountryID int REFERENCES [ExchangeRateFYP].Countries(CountryID),
		ToCountryRate decimal(10,2) null,
		CustomerID int REFERENCES [ExchangeRateFYP].Users(UserID),
		PaymentMethod nvarchar(50) null,
		InputMoneyAmount decimal(10,2) null,
		TotalCostAmount decimal(10,2) null,
		IsConfirmed bit null,
		Remarks nvarchar(50) null,
		IsActive bit,
		CreatedBy int,
		CreatedDate datetime,
		ModifiedBy int,
		ModifiedDate datetime
	)
END
--Enquiries
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES(NolocK) where TABLE_SCHEMA = 'ExchangeRateFYP' and TABLE_NAME ='Enquiries')
BEGIN
	CREATE TABLE [ExchangeRateFYP].Enquiries (
		EnquiryID INT IDENTITY PRIMARY KEY,
		EnquirySubject nvarchar(500) NULL,
		MessageID int NULL,
		EnquiryMessage nvarchar(1000) NULL,
		CustomerID int REFERENCES [ExchangeRateFYP].Users(UserID) null,
		TransactionID int REFERENCES [ExchangeRateFYP].Transactions(TransactionID)  null,
		Remarks nvarchar(50) null,
		IsActive bit,
		CreatedBy int,
		CreatedDate datetime,
		ModifiedBy int,
		ModifiedDate datetime
	)
END

DROP PROCEDURE [ExchangeRateFYP].InsertTransactions
DROP PROCEDURE [ExchangeRateFYP].UpdateTransactions
DROP PROCEDURE [ExchangeRateFYP].DeleteTransactions
DROP PROCEDURE [ExchangeRateFYP].InsertEnquiries

CREATE PROCEDURE [ExchangeRateFYP].[InsertTransactions]
@FromCountryID int null, @FromCountryRate DECIMAL NULL, 
@ToCountryID int null, @ToCountryRate DECIMAL null, 
@CustomerID int null, @PaymentMethod nvarchar(50) null,
@InputMoneyAmount decimal null, @TotalCostAmount decimal null,
@Remarks nvarchar(500) null
as
BEGIN
INSERT INTO [ExchangeRateFYP].[Transactions]
           ([FromCountryID]
           ,[FromCountryRate]
           ,[ToCountryID]
           ,[ToCountryRate]
           ,[CustomerID]
           ,[PaymentMethod]
           ,[InputMoneyAmount]
           ,[TotalCostAmount]
           ,[IsConfirmed]
           ,[Remarks]
           ,[IsActive]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
VALUES (
  @FromCountryID, @FromCountryRate, @ToCountryID, @ToCountryRate,
  @CustomerID, @PaymentMethod, @InputMoneyAmount, @TotalCostAmount, 0, @Remarks,1,1,GETDATE(),1,GETDATE()
)
END
GO

CREATE PROCEDURE [ExchangeRateFYP].[UpdateTransactions]
@FromCountryID int null, @FromCountryRate DECIMAL NULL, 
@ToCountryID int null, @ToCountryRate DECIMAL null, 
@CustomerID int null, @PaymentMethod nvarchar(50) null,
@InputMoneyAmount decimal null, @TotalCostAmount decimal null,
@TransactionID int, @IsActive bit null, @Remarks nvarchar(500) null
as
BEGIN
	UPDATE [ExchangeRateFYP].[Transactions]
	SET 
	  FromCountryID =  @FromCountryID, 
	  FromCountryRate = @FromCountryRate,
	  ToCountryID = @ToCountryID, 
	  ToCountryRate =  @ToCountryRate,
	  CustomerID =  @CustomerID, 
	  PaymentMethod =  @PaymentMethod, 
	  InputMoneyAmount = @InputMoneyAmount, 
	  TotalCostAmount =  @TotalCostAmount,
	  Remarks = @Remarks,
	  IsActive = @IsActive,
	  ModifiedDate = GETDATE()
	WHERE TransactionID = @TransactionID
END
GO

CREATE PROCEDURE [ExchangeRateFYP].[DeleteTransactions]
@TransactionID int 
as
BEGIN
	UPDATE [ExchangeRateFYP].[Transactions]
	SET 
      IsActive = 0,
	  ModifiedDate = GETDATE()
	WHERE TransactionID = @TransactionID
END


SET ANSI_NULLS ON
GO

CREATE PROCEDURE [ExchangeRateFYP].[InsertEnquiries]
@EnquirySubject nvarchar(500) null, @MessageID int null, 
@EnquiryMessage nvarchar(1000) NULL, @CustomerID int null,
@TransactionID int null,
@Remarks nvarchar(500) null,
@CreatedBy int null
as
BEGIN
IF (@MessageID IS NULL)
BEGIN
SET @MessageID = (select (ISNULL(MAX(MessageID),0)+1) from [ExchangeRateFYP].Enquiries(NolocK))
END

INSERT INTO [ExchangeRateFYP].[Enquiries]
           ([EnquirySubject]
		   ,[MessageID]
		   ,[EnquiryMessage]
		   ,[CustomerID]
		   ,[TransactionID]
           ,[Remarks]
           ,[IsActive]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
VALUES (
  @EnquirySubject,@MessageID,@EnquiryMessage,@CustomerID,@TransactionID,@Remarks,1,@CreatedBy,GETDATE(),@CreatedBy,GETDATE()
)
END 
GO
