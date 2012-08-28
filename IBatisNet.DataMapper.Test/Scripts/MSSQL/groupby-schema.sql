if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_User_Address]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Users] DROP CONSTRAINT FK_User_Address


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ApplicationUserRoles_Application]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ApplicationUserRoles] DROP CONSTRAINT FK_ApplicationUserRoles_Application


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ApplicationUsers_Application]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ApplicationUsers] DROP CONSTRAINT FK_ApplicationUsers_Application


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Application_Role]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Applications] DROP CONSTRAINT FK_Application_Role


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ApplicationUserRoles_Role]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ApplicationUserRoles] DROP CONSTRAINT FK_ApplicationUserRoles_Role


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ApplicationUserRoles_User]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ApplicationUserRoles] DROP CONSTRAINT FK_ApplicationUserRoles_User


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ApplicationUsers_User]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ApplicationUsers] DROP CONSTRAINT FK_ApplicationUsers_User


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Addresses]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Addresses]


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ApplicationUserRoles]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ApplicationUserRoles]


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ApplicationUsers]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ApplicationUsers]


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Applications]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Applications]


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Roles ]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Roles ]


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Users]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Users]


CREATE TABLE [dbo].[Addresses] (
	[AddressId] [int] NOT NULL ,
	[Street] [varchar] (50) COLLATE Latin1_General_CI_AS NULL 
) ON [PRIMARY]


CREATE TABLE [dbo].[ApplicationUserRoles] (
	[ApplicationId] [int] NULL ,
	[UserId] [int] NULL ,
	[RoleId] [int] NULL 
) ON [PRIMARY]


CREATE TABLE [dbo].[ApplicationUsers] (
	[ApplicationId] [int] NULL ,
	[UserId] [int] NULL ,
	[ActiveFlag] [bit] NULL 
) ON [PRIMARY]


CREATE TABLE [dbo].[Applications] (
	[ApplicationId] [int] NOT NULL ,
	[ApplicationName] [varchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[DefaultRoleId] [int] NULL 
) ON [PRIMARY]


CREATE TABLE [dbo].[Roles ] (
	[RoleId] [int] NOT NULL ,
	[RoleName] [varchar] (50) COLLATE Latin1_General_CI_AS NULL 
) ON [PRIMARY]


CREATE TABLE [dbo].[Users] (
	[UserId] [int] NOT NULL ,
	[UserName] [varchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[AddressId] [int] NULL 
) ON [PRIMARY]


ALTER TABLE [dbo].[Addresses] WITH NOCHECK ADD 
	CONSTRAINT [PK_Address] PRIMARY KEY  CLUSTERED 
	(
		[AddressId]
	)  ON [PRIMARY] 


ALTER TABLE [dbo].[Applications] WITH NOCHECK ADD 
	CONSTRAINT [PK_Application] PRIMARY KEY  CLUSTERED 
	(
		[ApplicationId]
	)  ON [PRIMARY] 


ALTER TABLE [dbo].[Roles ] WITH NOCHECK ADD 
	CONSTRAINT [PK_Role] PRIMARY KEY  CLUSTERED 
	(
		[RoleId]
	)  ON [PRIMARY] 


ALTER TABLE [dbo].[Users] WITH NOCHECK ADD 
	CONSTRAINT [PK_User] PRIMARY KEY  CLUSTERED 
	(
		[UserId]
	)  ON [PRIMARY] 


ALTER TABLE [dbo].[ApplicationUserRoles] ADD 
	CONSTRAINT [FK_ApplicationUserRoles_Application] FOREIGN KEY 
	(
		[ApplicationId]
	) REFERENCES [dbo].[Applications] (
		[ApplicationId]
	),
	CONSTRAINT [FK_ApplicationUserRoles_Role] FOREIGN KEY 
	(
		[RoleId]
	) REFERENCES [dbo].[Roles ] (
		[RoleId]
	),
	CONSTRAINT [FK_ApplicationUserRoles_User] FOREIGN KEY 
	(
		[UserId]
	) REFERENCES [dbo].[Users] (
		[UserId]
	)


ALTER TABLE [dbo].[ApplicationUsers] ADD 
	CONSTRAINT [FK_ApplicationUsers_Application] FOREIGN KEY 
	(
		[ApplicationId]
	) REFERENCES [dbo].[Applications] (
		[ApplicationId]
	),
	CONSTRAINT [FK_ApplicationUsers_User] FOREIGN KEY 
	(
		[UserId]
	) REFERENCES [dbo].[Users] (
		[UserId]
	)


ALTER TABLE [dbo].[Applications] ADD 
	CONSTRAINT [FK_Application_Role] FOREIGN KEY 
	(
		[DefaultRoleId]
	) REFERENCES [dbo].[Roles ] (
		[RoleId]
	)


ALTER TABLE [dbo].[Users] ADD 
	CONSTRAINT [FK_User_Address] FOREIGN KEY 
	(
		[AddressId]
	) REFERENCES [dbo].[Addresses] (
		[AddressId]
	)


