use [IBatisNet]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Coupons]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	drop table [dbo].[Coupons]
END
CREATE TABLE [dbo].[Coupons] (
	[Coupon_ID] [int] NOT NULL ,
	[Coupon_Code] [varchar] (32)  NOT NULL 
) ON [PRIMARY]

ALTER TABLE [dbo].[Coupons] WITH NOCHECK ADD 
	CONSTRAINT [PK_Coupons] PRIMARY KEY  CLUSTERED 
	(
		[Coupon_ID]
	)  ON [PRIMARY] 

-- Creating Test Data

INSERT INTO [dbo].[Coupons] VALUES(1,'AAA' );
INSERT INTO [dbo].[Coupons] VALUES(2,'BBB');
INSERT INTO [dbo].[Coupons] VALUES(3,'CCC');
INSERT INTO [dbo].[Coupons] VALUES(4,'DDD');
INSERT INTO [dbo].[Coupons] VALUES(5,'EEE');

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Coupons_Brands]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	drop table [dbo].[Coupons_Brands]
END
CREATE TABLE [dbo].[Coupons_Brands] (
	[Coupon_ID] [int] NOT NULL ,
	[Brand_Id] [int] NOT NULL 
) ON [PRIMARY]

-- Creating Test Data

INSERT INTO [dbo].[Coupons_Brands] VALUES(1,1 );
INSERT INTO [dbo].[Coupons_Brands] VALUES(1,2);
INSERT INTO [dbo].[Coupons_Brands] VALUES(1,3);
INSERT INTO [dbo].[Coupons_Brands] VALUES(2,4);
INSERT INTO [dbo].[Coupons_Brands] VALUES(2,5);
INSERT INTO [dbo].[Coupons_Brands] VALUES(5,6);