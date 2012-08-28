-- Creating Table

use [IBatisNet]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Enumerations]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	drop table [dbo].[Enumerations]
END

CREATE TABLE [dbo].[Enumerations] (
	[Enum_ID] [int] NOT NULL ,
	[Enum_Day] [int] NOT NULL ,
	[Enum_Color] [int] NOT NULL,
	[Enum_Month] [int] NULL,
	[Enum_SearchProfile] [varchar](1) NULL
) ON [PRIMARY]

ALTER TABLE [dbo].[Enumerations] WITH NOCHECK ADD 
	CONSTRAINT [PK_Enum] PRIMARY KEY  CLUSTERED 
	(
		[Enum_ID]
	)  ON [PRIMARY] 

-- Creating Test Data

INSERT INTO [dbo].[Enumerations] VALUES(1, 1, 1, 128, 'T');
INSERT INTO [dbo].[Enumerations] VALUES(2, 2, 2, 2048, 'P');
INSERT INTO [dbo].[Enumerations] VALUES(3, 3, 4, 256, 'P');
INSERT INTO [dbo].[Enumerations] VALUES(4, 4, 8, null, 'P');


