CREATE OR REPLACE PACKAGE BODY REF_CURSOR_TEST IS

	FUNCTION GET_ACCOUNTS_FUNCTION RETURN T_ACCOUNTS_CURSOR 
	IS
	C_ACCOUNTS T_ACCOUNTS_CURSOR;
	BEGIN
		OPEN C_ACCOUNTS FOR
			SELECT
				Account_ID as Id,
				Account_FirstName as FirstName,
				Account_LastName as LastName,
				Account_Email as EmailAddress
			FROM Accounts
			ORDER BY Account_ID;
		RETURN C_ACCOUNTS;
	END GET_ACCOUNTS_FUNCTION;

	PROCEDURE GET_ACCOUNTS_PROCEDURE (
			P_ACCOUNTS OUT T_ACCOUNTS_CURSOR
	) AS
	BEGIN
	OPEN P_ACCOUNTS FOR 
			SELECT
				Account_ID as Id,
				Account_FirstName as FirstName,
				Account_LastName as LastName,
				Account_Email as EmailAddress
			FROM Accounts
			ORDER BY Account_ID;
	END GET_ACCOUNTS_PROCEDURE;
	
	PROCEDURE GET_ACCOUNT_PROCEDURE (
		P_ACCOUNTS OUT T_ACCOUNTS_CURSOR, 
		P_ACCOUNT_ID IN INTEGER
	) AS
	BEGIN
	OPEN P_ACCOUNTS FOR 
			SELECT
				Account_ID as Id,
				Account_FirstName as FirstName,
				Account_LastName as LastName,
				Account_Email as EmailAddress
			FROM Accounts
			WHERE Account_ID = P_ACCOUNT_ID
			ORDER BY Account_ID;
	END GET_ACCOUNT_PROCEDURE;

END REF_CURSOR_TEST;