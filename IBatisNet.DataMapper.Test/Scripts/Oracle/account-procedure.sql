CREATE OR REPLACE PROCEDURE prc_InsertAccount (	
	p_Account_ID IN INTEGER, 
	p_Account_FirstName IN VARCHAR2, 
	p_Account_LastName IN VARCHAR2, 
	p_Account_Email IN VARCHAR2,
	p_Account_Banner_Option IN VARCHAR2,
	p_Account_Cart_Option IN INTEGER ) 
IS 
BEGIN 
	INSERT INTO Accounts (	
		Account_ID, 
		Account_FirstName, 
		Account_LastName, 
		Account_Email,
		Account_Banner_Option,
		Account_Cart_Option ) 
	VALUES ( 
		p_Account_ID, 
		p_Account_FirstName, 
		p_Account_LastName, 
		p_Account_Email,
		p_Account_Banner_Option,
		p_Account_Cart_Option ); 
END;