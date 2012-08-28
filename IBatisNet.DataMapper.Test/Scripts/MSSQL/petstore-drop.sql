use [IBatisNet]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Accounts]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Orders_Accounts]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
	ALTER TABLE [dbo].[Orders] DROP CONSTRAINT FK_Orders_Accounts

	drop table [dbo].[Accounts]
END

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Profiles]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Profiles]


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SignsOn]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[SignsOn]


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Orders]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_LineItems_Orders]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
	ALTER TABLE [dbo].[LineItems] DROP CONSTRAINT FK_LineItems_Orders

	drop table [dbo].[Orders]
END

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Categories]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Categories]


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Inventories]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Inventories]


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Items]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Items]


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LinesItem]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[LinesItem]


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Products]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Products]


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Suppliers]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Suppliers]


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sequences]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Sequences]