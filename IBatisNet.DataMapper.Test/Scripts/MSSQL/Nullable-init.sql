use [IBatisNet]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Nullable]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	drop table [dbo].[Nullable]
END

CREATE TABLE [dbo].[Nullable] (
	[Id] [int] IDENTITY (1, 1) NOT NULL ,
	[TestBool] [Bit] NULL ,
	[TestByte] [tinyint] NULL ,
	[TestChar] [char] NULL ,
	[TestDateTime] [datetime] NULL ,
	[TestDecimal] decimal(9,2) NULL ,
	[TestDouble] [float] NULL ,
	[TestGuid] [UniqueIdentifier] NULL ,
	[TestInt16] [SmallInt] NULL ,
	[TestInt32] [int] NULL ,
	[TestInt64] [bigint] NULL ,
	[TestSingle] [real] NULL ,
	[TestTimeSpan] [bigint] NULL,
	CONSTRAINT [PK_Nullable] PRIMARY KEY  CLUSTERED 
	(
		[Id]
	)  ON [PRIMARY] 
) ON [PRIMARY]
