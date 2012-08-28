CREATE OR REPLACE PROCEDURE prc_SWAP_EMAIL_ADDRESS ( 
	p_first IN OUT VARCHAR2, 
	p_second IN OUT VARCHAR2 ) 
IS 
	v_id1 NUMBER (5,0); 
	v_id2 NUMBER (5,0); 
	v_email1 VARCHAR2 (255); 
	v_email2 VARCHAR2 (255); 
BEGIN 
	SELECT 
		ACCOUNT_ID, 
		ACCOUNT_EMAIL 
	INTO v_id1, 
		v_email1 
	FROM 
		ACCOUNTS 
	WHERE 
		ACCOUNT_EMAIL = p_first; 
		
	SELECT 
		ACCOUNT_ID, 
		ACCOUNT_EMAIL 
	INTO 
		v_id2, 
		v_email2 
	FROM 
		ACCOUNTS 
	WHERE 
		ACCOUNT_EMAIL = p_second; 
		
	UPDATE 
		ACCOUNTS 
	SET 
		ACCOUNT_EMAIL = v_email2 
	WHERE 
		ACCOUNT_ID = v_id1; 
		
	UPDATE 
		ACCOUNTS 
	SET 
		ACCOUNT_EMAIL = v_email1 
	WHERE 
		ACCOUNT_ID = v_id2; 
		
	SELECT 
		ACCOUNT_EMAIL 
	INTO 
		p_first 
	FROM 
		ACCOUNTS 
	WHERE 
		ACCOUNT_ID = v_id1; 
		
	SELECT 
		ACCOUNT_EMAIL 
	INTO 
		p_second 
	FROM 
		ACCOUNTS 
	WHERE 
		ACCOUNT_ID = v_id2; 
END;