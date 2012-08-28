CREATE OR REPLACE PROCEDURE prc_InsertCategory ( 
	p_Category_Name IN VARCHAR2, 
	p_Category_Guid IN VARCHAR2, 
	p_Category_Id OUT INT ) 
IS 
BEGIN 
	SELECT S_CATEGORIES.nextval 
	INTO p_Category_Id 
	FROM DUAL; 
	
	INSERT INTO Categories ( 
		Category_Id,	
		Category_Name, 
		Category_Guid ) 
	VALUES (	
		p_Category_Id, 
		p_Category_Name, 
		p_Category_Guid ); 
END;