USE master

GO

IF exists (SELECT * FROM sysdatabases WHERE name= 'QuanLyQuanCafe')

DROP DATABASE QuanLyQuanCafe

GO
CREATE DATABASE QuanLyQuanCafe
GO
USE QuanLyQuanCafe
GO

-- Food
-- Table
-- FoodCategory
-- Account
-- Bill
-- BillInfo

CREATE TABLE TableFood
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Bàn chưa có tên',
	status NVARCHAR(100) NOT NULL DEFAULT N'Trống'	-- Trống || Có người
)
GO

CREATE TABLE Account
(
	UserName NVARCHAR(100) PRIMARY KEY,	
	DisplayName NVARCHAR(100) NOT NULL DEFAULT N'Nhân viên',
	PassWord NVARCHAR(1000) NOT NULL DEFAULT 0,
	Type INT NOT NULL  DEFAULT 0 -- 1: admin && 0: staff
)
GO

CREATE TABLE FoodCategory
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên'
)
GO

CREATE TABLE Food
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên',
	idCategory INT NOT NULL,
	price FLOAT NOT NULL DEFAULT 0
	
	FOREIGN KEY (idCategory) REFERENCES dbo.FoodCategory(id)
)
GO

CREATE TABLE Bill
(
	id INT IDENTITY PRIMARY KEY,
	DateCheckIn DATE NOT NULL DEFAULT GETDATE(),
	DateCheckOut DATE,
	idTable INT NOT NULL,
	status INT NOT NULL DEFAULT 0 -- 1: đã thanh toán && 0: chưa thanh toán
	
	FOREIGN KEY (idTable) REFERENCES dbo.TableFood(id)
)
GO

CREATE TABLE BillInfo
(
	id INT IDENTITY PRIMARY KEY,
	idBill INT NOT NULL,
	idFood INT NOT NULL,
	count INT NOT NULL DEFAULT 0
	
	FOREIGN KEY (idBill) REFERENCES dbo.Bill(id),
	FOREIGN KEY (idFood) REFERENCES dbo.Food(id)
)
GO
INSERT INTO Account
      (Username,
	   DisplayName,
	   PassWord,
	   Type
	   )
VALUES  ( N'Customer' , -- UserName - nvarchar(100)
          N'Customer' , -- DisplayName - nvarchar(100)
          N'1' , -- PassWord - nvarchar(1000)
          1  -- Type - int
        )
INSERT INTO Account
        ( UserName ,
          DisplayName ,
          PassWord ,
          Type
        )
VALUES  ( N'Staff' , -- UserName - nvarchar(100)
          N'Staff' , -- DisplayName - nvarchar(100)
          N'1' , -- PassWord - nvarchar(1000)
          0  -- Type - int
        )
GO
Create proc USP_GetListAccountByUsername
@username nvarchar(100)
AS
BEGIN 
    select * from dbo.Account where UserName= @username
END
GO

EXEC dbo.USP_GetListAccountByUsername @username = N'Customer' -- nvarchar(100)

GO
Create proc USP_Login
@username nvarchar(100), @password nvarchar (100)
AS
BEGIN
	Select * from dbo.Account where UserName=@username and PassWord=@password
END
GO
--Thêm bàn
DECLARE @i INT = 0

WHILE @i <= 10
BEGIN
	INSERT dbo.TableFood ( name)VALUES  ( N'Bàn ' + CAST(@i AS nvarchar(100)))
	SET @i = @i + 1
END
GO

Create proc USP_GetTableList
AS select * from dbo.TableFood
GO

UPDATE dbo.TableFood SET STATUS = N'Có người' WHERE id = 9

EXEC dbo.USP_GetTableList 
GO
-- thêm category
INSERT dbo.FoodCategory
        ( name )
VALUES  ( N'Hải sản'  -- name - nvarchar(100)
          )
INSERT dbo.FoodCategory
        ( name )
VALUES  ( N'Nông sản' )
INSERT dbo.FoodCategory
        ( name )
VALUES  ( N'Lâm sản' )
INSERT dbo.FoodCategory
        ( name )
VALUES  ( N'Bánh ngọt' )
INSERT dbo.FoodCategory
        ( name )
VALUES  ( N'Nước' )

-- thêm món ăn
INSERT dbo.Food
        ( name, idCategory, price )
VALUES  ( N'Mực một nắng nướng sa tế', -- name - nvarchar(100)
          1, -- idCategory - int
          120000)
INSERT dbo.Food
        ( name, idCategory, price )
VALUES  ( N'Nghêu hấp xả', 1, 50000)
INSERT dbo.Food
        ( name, idCategory, price )
VALUES  ( N'Cơm chiên', 2, 60000)
INSERT dbo.Food
        ( name, idCategory, price )
VALUES  ( N'Heo rừng nướng muối ớt', 3, 75000)
INSERT dbo.Food
        ( name, idCategory, price )
VALUES  ( N'Cơm chiên kimchi', 4, 999999)
INSERT dbo.Food
        ( name, idCategory, price )
VALUES  ( N'Cà phê đen', 5, 15000)
INSERT dbo.Food
        ( name, idCategory, price )
VALUES  ( N'Cafe', 5, 12000)

-- thêm bill
INSERT	dbo.Bill
        ( DateCheckIn ,
          DateCheckOut ,
          idTable ,
          status
        )
VALUES  ( GETDATE() , -- DateCheckIn - date
          NULL , -- DateCheckOut - date
          3 , -- idTable - int
          0  -- status - int
        )
        
INSERT	dbo.Bill
        ( DateCheckIn ,
          DateCheckOut ,
          idTable ,
          status
        )
VALUES  ( GETDATE() , -- DateCheckIn - date
          NULL , -- DateCheckOut - date
          4, -- idTable - int
          0  -- status - int
        )
INSERT	dbo.Bill
        ( DateCheckIn ,
          DateCheckOut ,
          idTable ,
          status
        )
VALUES  ( GETDATE() , -- DateCheckIn - date
          GETDATE() , -- DateCheckOut - date
          5 , -- idTable - int
          1  -- status - int
        )

-- thêm bill info
INSERT	dbo.BillInfo
        ( idBill, idFood, count )
VALUES  ( 5, -- idBill - int
          1, -- idFood - int
          2  -- count - int
          )
INSERT	dbo.BillInfo
        ( idBill, idFood, count )
VALUES  ( 5, -- idBill - int
          3, -- idFood - int
          4  -- count - int
          )
INSERT	dbo.BillInfo
        ( idBill, idFood, count )
VALUES  ( 5, -- idBill - int
          5, -- idFood - int
          1  -- count - int
          )
INSERT	dbo.BillInfo
        ( idBill, idFood, count )
VALUES  ( 6, -- idBill - int
          1, -- idFood - int
          2  -- count - int
          )
INSERT	dbo.BillInfo
        ( idBill, idFood, count )
VALUES  ( 6, -- idBill - int
          6, -- idFood - int
          2  -- count - int
          )
INSERT	dbo.BillInfo
        ( idBill, idFood, count )
VALUES  ( 7, -- idBill - int
          5, -- idFood - int
          2  -- count - int
          )         
          
GO
Create proc USP_InsertBill
@idTable INT
AS
BEGIN
   INSERT dbo.Bill
   (DateCheckIn,
    DateCheckOut,
	idTable,
	status)
	VALUES(GETDATE() , -- DateCheckIn - date
          NULL , -- DateCheckOut - date
          @idTable , -- idTable - int
          0  -- status - int
        )
END
GO

CREATE proc	USP_InsertBillInfo
@idBill INT, @idFood INT, @count INT
AS
BEGIN
 
	DECLARE @isExistBillInfo INT
	DECLARE @foodCount INT = 1
	Select @isExistBillInfo = id, @foodCount = b.count From dbo.BillInfo as b where idBill = @idBill and idFood = @idFood
	if (@isExistBillInfo >0)
	BEGIN
		DECLARE @newCount INT = @foodCount+ @count
		IF (@newCount >0)
		UPDATE	dbo.BillInfo SET Count = @foodCount + count where idFood = @idFood
		ELSE
		DELETE dbo.BillInfo where idBill = @idBill and idFood = @idFood
	END
	else  
	BEGIN
	INSERT	dbo.BillInfo
        ( idBill, idFood, count )
	VALUES  ( @idBill, -- idBill - int
          @idFood, -- idFood - int
          @count  -- count - int
          )
	END
END
GO

DELETE dbo.BillInfo
DELETE dbo.Bill 

Create trigger UTG_UpdateBillInfo
On dbo.BillInfo for insert, update
AS
BEGIN
	DECLARE	@idBill INT
	SELECT idBill from inserted
	DECLARE @idTable INT
	SELECT @idTable = idTable from dbo.Bill where id = @idBill and status = 0
	UPDATE dbo.TableFood SET status = N'Có người' Where	id = @idTable
END
GO

Create trigger UTG_UpdateBIll
ON dbo.Bill for Update 
AS
BEGIN
	DECLARE @idBill INT 
	SELECT @idBill = id from inserted
	DECLARE @idTable INT
	SELECT @idTable = idTable from dbo.Bill where id = @idBill
	DECLARE @count INT =0
	SELECT @count = Count(*) from dbo.Bill where idTable = @idTable and status = 0
	IF (@count ==0)
		UPDATE dbo.TableFood SET status = N'Trống' where id=@idTable
END
GO