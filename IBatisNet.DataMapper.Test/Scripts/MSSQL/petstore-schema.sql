use [IBatisNet]

-- ---------------------------------------------------------------------------------------------------
-- CREATE TABLE [Categories]
-- -------------------------------------------------------------------------------------------------*/

CREATE TABLE [dbo].[Categories] (
	[Category_Id] [varchar] (10) NOT NULL ,
	[Category_Name] [varchar] (80) NULL ,
	[Category_Description] [varchar] (255) NULL 
) ON [PRIMARY]


-- ---------------------------------------------------------------------------------------------------
-- CREATE TABLE [Products]
-- -------------------------------------------------------------------------------------------------*/

CREATE TABLE [dbo].[Products] (
	[Product_Id] [varchar] (10) NOT NULL ,
	[Category_Id] [varchar] (10) NOT NULL ,
	[Product_Name] [varchar] (80) NULL ,
	[Product_Description] [varchar] (255) NULL 
) ON [PRIMARY]


-- --------------------------------------------------------------------------------------------------
-- CREATE TABLE [Suppliers]
-- -------------------------------------------------------------------------------------------------*/

CREATE TABLE [dbo].[Suppliers] (
	[Supplier_Id] int PRIMARY KEY,
	[Supplier_Name] varchar(80) NULL,
	[Supplier_Status] varchar(2) NOT NULL,
	[Supplier_Addr1] varchar(80) NULL,
	[Supplier_Addr2] varchar(80) NULL,
	[Supplier_City] varchar(80) NULL,
	[Supplier_State] varchar(80) NULL,
	[Supplier_Zip] varchar(5) NULL,
	[Supplier_Phone] varchar(40) NULL 
)


-- ---------------------------------------------------------------------------------------------------
-- CREATE TABLE [Items] 
-- -------------------------------------------------------------------------------------------------*/

CREATE TABLE [dbo].[Items] (
	[Item_Id] varchar(10) PRIMARY KEY,
	[Product_Id] varchar(10) NOT NULL,
	[Item_ListPrice] decimal(10, 2) NULL,
	[Item_UnitCost] decimal(10, 2) NULL,
	[Supplier_Id] int NULL,
	[Item_Status] varchar(2) NULL,
	[Item_Attr1] varchar(80) NULL,
	[Item_Attr2] varchar(80) NULL,
	[Item_Attr3] varchar(80) NULL,
	[Item_Attr4] varchar(80) NULL,
	[Item_Attr5] varchar(80) NULL,
	[Item_Photo] varchar(80) NULL
)


-- ---------------------------------------------------------------------------------------------------
-- CREATE TABLE [Inventories] 
-- -------------------------------------------------------------------------------------------------*/

CREATE TABLE [dbo].[Inventories] (
	[Item_Id] [varchar] (10) NOT NULL,
	[Inventory_Quantity] [int] NOT NULL 
) ON [PRIMARY]


-- ---------------------------------------------------------------------------------------------------
-- CREATE TABLE [Accounts]
-- -------------------------------------------------------------------------------------------------*/

CREATE TABLE [dbo].[Accounts] (
	[Account_Id] varchar(20) PRIMARY KEY,
	[Account_Email] varchar(80) NOT NULL,
	[Account_FirstName] varchar(80) NOT NULL,
	[Account_LastName] varchar(80) NOT NULL,
	[Account_Status] varchar(2) NULL,
	[Account_Addr1] varchar(80) NOT NULL,
	[Account_Addr2] varchar(80) NULL,
	[Account_City] varchar(80) NOT NULL,
	[Account_State] varchar(80) NOT NULL,
	[Account_Zip] varchar(20) NOT NULL,
	[Account_Country] varchar(20) NOT NULL,
	[Account_Phone] varchar(20) NOT NULL
)

-- ---------------------------------------------------------------------------------------------------
-- CREATE TABLE [Profiles]
-- -------------------------------------------------------------------------------------------------*/

CREATE TABLE [dbo].[Profiles] (
	[Account_Id] varchar(20) PRIMARY KEY,
	[Profile_LangPref] varchar(80) NOT NULL,
	[Profile_FavCategory] varchar(10) NULL,
	[Profile_MyListOpt] bit NULL,
	[Profile_BannerOpt] bit NULL
)

-- ---------------------------------------------------------------------------------------------------
-- CREATE TABLE [SignsOn]
-- -------------------------------------------------------------------------------------------------*/

CREATE TABLE [dbo].[SignsOn] (
	[Account_Id] varchar(20) PRIMARY KEY,
	[SignOn_Password] varchar(20) NOT NULL
)

-- ---------------------------------------------------------------------------------------------------
-- CREATE TABLE [Orders]
-- -------------------------------------------------------------------------------------------------*/
CREATE TABLE [dbo].[Orders] (
	[Order_Id] [int] NOT NULL ,
	[Account_ID] varchar(20) NOT NULL ,
	[Order_Date] datetime NOT NULL ,
	[Order_ShipToFirstName] varchar(80) NOT NULL ,
	[Order_ShipToLastName] varchar(80) NOT NULL ,
	[Order_ShipAddr1] varchar(80) NOT NULL ,
	[Order_ShipAddr2] varchar(80) NULL ,
	[Order_ShipCity] varchar(80) NOT NULL ,
	[Order_ShipState] varchar(80) NOT NULL ,
	[Order_ShipZip] varchar(20) NOT NULL ,
	[Order_ShipCountry] varchar(20) NOT NULL ,
	[Order_BillToFirstName] varchar(80) NOT NULL ,
	[Order_BillToLastName] varchar(80) NOT NULL ,
	[Order_BillAddr1] varchar(80) NOT NULL ,
	[Order_BillAddr2] varchar(80) NULL ,
	[Order_BillCity] varchar(80) NOT NULL ,
	[Order_BillState] varchar(80) NOT NULL ,
	[Order_BillZip] varchar(20) NOT NULL ,
	[Order_BillCountry] varchar(20) NOT NULL ,
	[Order_TotalPrice] decimal(10, 2) NOT NULL ,
	[Order_CreditCard] varchar(20) NOT NULL ,
	[Order_ExprDate] varchar(7) NOT NULL ,
	[Order_CardType] varchar(40) NOT NULL 
) ON [PRIMARY]

-- ---------------------------------------------------------------------------------------------------
-- CREATE TABLE [LinesItem]
-- -------------------------------------------------------------------------------------------------
CREATE TABLE [dbo].[LinesItem] (
	[Order_Id] int NOT NULL ,
	[LineItem_LineNum] int NOT NULL ,
	[Item_Id] varchar(10) NOT NULL ,
	[LineItem_Quantity] int NOT NULL ,
	[LineItem_UnitPrice] decimal(10, 2) NOT NULL 
) ON [PRIMARY]

-- ---------------------------------------------------------------------------------------------------
-- CREATE TABLE [Sequences]
-- -------------------------------------------------------------------------------------------------
CREATE TABLE [dbo].[Sequences] (
	[Sequence_Name] [varchar] (30) NOT NULL ,
	[Sequence_NextId] [int] NOT NULL 
) ON [PRIMARY]
