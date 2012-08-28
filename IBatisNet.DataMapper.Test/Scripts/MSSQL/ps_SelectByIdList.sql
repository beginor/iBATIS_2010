

CREATE PROCEDURE ps_SelectByIdList(@AccountIds xml) AS

DECLARE @Ids TABLE (ID int) 

INSERT INTO @Ids (ID) SELECT ParamValues.ID.value('.','int')
FROM @AccountIds.nodes('/Accounts/id') as ParamValues(ID) 

SELECT 
	Account_ID as Id,
	Account_FirstName as FirstName,
	Account_LastName as LastName,
	Account_Email as EmailAddress
FROM 
    Accounts
INNER JOIN 
    @Ids p
ON    Accounts.Account_ID = p.ID