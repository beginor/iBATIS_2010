-- Creating Table

use [IBatisNet]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Users]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	drop table [dbo].[Users]
END

CREATE TABLE [dbo].[Users] (
  [User_ID] [int] NOT NULL ,
  [Name] [varchar] (32)  NOT NULL
  PRIMARY KEY  (User_ID)
)

-- Creating Test Data

INSERT INTO [dbo].[Users] VALUES(1,'Joe');
INSERT INTO [dbo].[Users] VALUES(2,'Averel');
INSERT INTO [dbo].[Users] VALUES(3,'William');
INSERT INTO [dbo].[Users] VALUES(4,'Jack');
INSERT INTO [dbo].[Users] VALUES(5,'Gilles');

