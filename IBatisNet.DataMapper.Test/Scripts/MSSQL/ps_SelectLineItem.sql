CREATE PROCEDURE dbo.[ps_SelectLineItem]
@Order_ID  [int]
AS
select
	LineItem_ID as Id,
	LineItem_Code as Code,
	LineItem_Quantity as Quantity,
	LineItem_Price as Price
from LineItems where Order_ID = @Order_ID