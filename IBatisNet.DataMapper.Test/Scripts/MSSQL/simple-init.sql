-- Creating Table

use [IBatisNet]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Simples]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	drop table [dbo].[Simples]
END

CREATE TABLE [dbo].[Simples] (
	[ID] [int] NOT NULL ,
	[Name] [varchar] (64) NULL ,
	[Address] [varchar] (64) NULL ,
	[Count] [int] NULL ,
	[Date] [datetime] NULL ,
	[Pay] [decimal](18, 2) NULL,
  PRIMARY KEY  (ID)
)