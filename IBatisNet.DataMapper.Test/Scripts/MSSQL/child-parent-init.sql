-- Creating Table

use [IBatisNet]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Child]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
begin
drop table [dbo].[Child]
end

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Parent]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
begin
drop table [dbo].[Parent]
end

CREATE TABLE [dbo].[Child] (
	[Id] [int] IDENTITY (1, 1) NOT NULL ,
	[ParentId] [int] NOT NULL ,
	[Description] [varchar] (100) NOT NULL ,
	[RowVersion] [timestamp] NOT NULL 
) ON [PRIMARY]

CREATE TABLE [dbo].[Parent] (
	[Id] [int] IDENTITY (1, 1) NOT NULL ,
	[Description] [varchar] (100) NOT NULL ,
	[RowVersion] [timestamp] NOT NULL 
) ON [PRIMARY]

-- Creating Test Data
insert into Parent ([Description]) values ('Parent 1')
insert into Parent ([Description]) values ('Parent 2')

insert into Child ([ParentId], [Description]) values (1, 'Child 1')
insert into Child ([ParentId], [Description]) values (1, 'Child 2')