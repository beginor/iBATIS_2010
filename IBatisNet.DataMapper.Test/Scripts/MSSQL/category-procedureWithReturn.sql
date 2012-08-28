CREATE PROCEDURE dbo.[ps_InsertCategorieWithReturnValue]
@Category_Id [int] output,
@Category_Name [varchar] (32),
@Category_Guid [uniqueidentifier] 
AS
insert into Categories  
			(Category_Name, Category_Guid ) 
values 
			(@Category_Name, @Category_Guid)
set @Category_Id = SCOPE_IDENTITY()
return @Category_Id

