CREATE PROCEDURE dbo.[ps_SelectAllAccount]
AS
select
	Account_ID as Id,
	Account_FirstName as FirstName,
	Account_LastName as LastName,
	Account_Email as EmailAddress
from Accounts
