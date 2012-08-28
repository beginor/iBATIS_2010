/*==============================================================*/
/* Nom de la base :  ORACLE                                     */
/* Nom de SGBD :  ORACLE Version 9i                             */
/* Date de création :  27/05/2004 20:55:37                      */
/*==============================================================*/


DROP TABLE ACCOUNTS CASCADE CONSTRAINTS;

DROP TABLE CATEGORIES CASCADE CONSTRAINTS;

DROP TABLE ENUMERATIONS CASCADE CONSTRAINTS;

DROP TABLE LINEITEMS CASCADE CONSTRAINTS;

DROP TABLE ORDERS CASCADE CONSTRAINTS;

DROP TABLE OTHERS CASCADE CONSTRAINTS;

DROP TABLE A CASCADE CONSTRAINTS;

DROP TABLE B CASCADE CONSTRAINTS;

DROP TABLE C CASCADE CONSTRAINTS;

DROP TABLE D CASCADE CONSTRAINTS;

DROP TABLE E CASCADE CONSTRAINTS;

DROP TABLE F CASCADE CONSTRAINTS;

DROP TABLE DOCUMENTS CASCADE CONSTRAINTS;

DROP TABLE SIMPLES CASCADE CONSTRAINTS;

DROP TABLE NULLABLE CASCADE CONSTRAINTS;

DROP SEQUENCE S_CATEGORIES;

DROP SEQUENCE S_NULLABLE;

CREATE SEQUENCE S_CATEGORIES
  START WITH 1
  MAXVALUE 1E27
  MINVALUE 1
  NOCYCLE
  CACHE 20
  NOORDER;

CREATE SEQUENCE S_NULLABLE
  START WITH 1
  MAXVALUE 1E27
  MINVALUE 1
  NOCYCLE
  CACHE 20
  NOORDER;

/*==============================================================*/
/* Table : ACCOUNTS                                             */
/*==============================================================*/


CREATE TABLE ACCOUNTS  (
   ACCOUNT_ID				INTEGER                          NOT NULL,
   ACCOUNT_FIRSTNAME		VARCHAR2(32)                     NOT NULL,
   ACCOUNT_LASTNAME			VARCHAR2(32)                     NOT NULL,
   ACCOUNT_EMAIL			VARCHAR2(128),
   ACCOUNT_BANNER_OPTION	VARCHAR2(255),
   ACCOUNT_CART_OPTION		INTEGER,   
   CONSTRAINT PK_ACCOUNTS PRIMARY KEY (ACCOUNT_ID)
)
NOLOGGING 
NOCACHE 
NOPARALLEL;

/*==============================================================*/
/* Table : CATEGORIES                                           */
/*==============================================================*/


CREATE TABLE CATEGORIES  (
   CATEGORY_ID          NUMBER(6)                        NOT NULL,
   CATEGORY_NAME        VARCHAR2(32),
   CATEGORY_GUID        VARCHAR2(36)
)
NOLOGGING
NOCACHE
NOPARALLEL;

/*==============================================================*/
/* Table : ENUMERATIONS                                         */
/*==============================================================*/


CREATE TABLE ENUMERATIONS  (
   ENUM_ID              INTEGER                          NOT NULL,
   ENUM_DAY             INTEGER                          NOT NULL,
   ENUM_COLOR           INTEGER                          NOT NULL,
   ENUM_MONTH           INTEGER,
   CONSTRAINT PK_ENUMERATIONS PRIMARY KEY (ENUM_ID)
)
NOLOGGING
NOCACHE
NOPARALLEL;

/*==============================================================*/
/* Table : LINEITEMS                                            */
/*==============================================================*/


CREATE TABLE LINEITEMS  (
   LINEITEM_ID          INTEGER                          NOT NULL,
   ORDER_ID             INTEGER                          NOT NULL,
   LINEITEM_CODE        VARCHAR2(32)                     NOT NULL,
   LINEITEM_QUANTITY    INTEGER                          NOT NULL,
   LINEITEM_PRICE       NUMBER(18,2),
   LINEITEM_PICTURE		BLOB,
   CONSTRAINT PK_LINEITEMS PRIMARY KEY (ORDER_ID, LINEITEM_ID)
)
NOLOGGING
NOCACHE
NOPARALLEL;

/*==============================================================*/
/* Table : ORDERS                                               */
/*==============================================================*/


CREATE TABLE ORDERS  (
   ORDER_ID             INTEGER                          NOT NULL,
   ACCOUNT_ID           INTEGER                          NULL,
   ORDER_DATE           DATE,
   ORDER_CARDTYPE       VARCHAR2(32),
   ORDER_CARDNUMBER     VARCHAR2(32),
   ORDER_CARDEXPIRY     VARCHAR2(32),
   ORDER_STREET         VARCHAR2(32),
   ORDER_CITY           VARCHAR2(32),
   ORDER_PROVINCE       VARCHAR2(32),
   ORDER_POSTALCODE     VARCHAR2(32),
   ORDER_FAVOURITELINEITEM INTEGER,
   CONSTRAINT PK_ORDERS PRIMARY KEY (ORDER_ID)
)
NOLOGGING
NOCACHE
NOPARALLEL;

/*==============================================================*/
/* Table : OTHERS                                               */
/*==============================================================*/


CREATE TABLE OTHERS (
	OTHER_INT INT NULL ,
	OTHER_LONG INT NULL ,
	OTHER_BIT SMALLINT DEFAULT 0 NOT NULL,
	OTHER_STRING VARCHAR2(32) NOT NULL 	
)
NOLOGGING
NOCACHE
NOPARALLEL;

CREATE TABLE F (
	ID	VARCHAR2(50) NOT NULL ,
	F_LIBELLE VARCHAR2(50) NULL ,
	CONSTRAINT PK_F PRIMARY KEY (ID)
)
NOLOGGING 
NOCACHE 
NOPARALLEL;

CREATE TABLE E (
	ID	VARCHAR2(50) NOT NULL ,
	E_LIBELLE VARCHAR2(50) NULL ,
	CONSTRAINT PK_E PRIMARY KEY (ID)
)
NOLOGGING 
NOCACHE 
NOPARALLEL;

CREATE TABLE D (
	ID	VARCHAR2(50) NOT NULL ,
	D_LIBELLE VARCHAR2(50) NULL ,
	CONSTRAINT PK_D PRIMARY KEY (ID)
)
NOLOGGING 
NOCACHE 
NOPARALLEL;

CREATE TABLE C (
	ID	VARCHAR2(50) NOT NULL ,
	C_LIBELLE VARCHAR2(50) NULL ,
	CONSTRAINT PK_C PRIMARY KEY (ID)
)
NOLOGGING 
NOCACHE 
NOPARALLEL;

CREATE TABLE B (
	ID VARCHAR2(50) NOT NULL ,
	C_ID VARCHAR2(50) NULL ,
	D_ID VARCHAR2(50) NULL ,
	B_LIBELLE VARCHAR2(50) NULL ,
	CONSTRAINT PK_B PRIMARY KEY (ID)
)
NOLOGGING 
NOCACHE 
NOPARALLEL;

ALTER TABLE B
  ADD CONSTRAINT FK_B_C
    FOREIGN KEY (C_ID)
    REFERENCES C (ID);

ALTER TABLE B
  ADD CONSTRAINT FK_B_D
    FOREIGN KEY (D_ID)
    REFERENCES D (ID);    
    
CREATE TABLE A (
	ID VARCHAR2(50) NOT NULL ,
	B_ID VARCHAR2(50)  NULL ,
	E_ID VARCHAR2(50)  NULL ,
	F_ID VARCHAR2(50)  NULL ,
	A_LIBELLE VARCHAR2(50) NULL ,
	CONSTRAINT PK_A PRIMARY KEY (ID)
)
NOLOGGING 
NOCACHE 
NOPARALLEL;

ALTER TABLE A
  ADD CONSTRAINT FK_A_B
    FOREIGN KEY (B_ID)
    REFERENCES B (ID);

ALTER TABLE A
  ADD CONSTRAINT FK_A_E
    FOREIGN KEY (E_ID)
    REFERENCES E (ID);    

ALTER TABLE A
  ADD CONSTRAINT FK_A_F
    FOREIGN KEY (F_ID)
    REFERENCES F (ID);    


/*==============================================================*/
/* Table : DOCUMENTS                                            */
/*==============================================================*/


CREATE TABLE DOCUMENTS  (
   DOCUMENT_ID           INTEGER                          NOT NULL,
   DOCUMENT_TITLE        VARCHAR2(32),
   DOCUMENT_TYPE         VARCHAR2(32),
   DOCUMENT_PAGENUMBER   INTEGER,
   DOCUMENT_CITY         VARCHAR2(32),
   CONSTRAINT PK_DOCUMENTS PRIMARY KEY (DOCUMENT_ID)
)
NOLOGGING 
NOCACHE 
NOPARALLEL;


/*==============================================================*/
/* Table : SIMPLES                                              */
/*==============================================================*/


CREATE TABLE SIMPLES (
   ID INTEGER NOT NULL,
   NAME VARCHAR(64) NULL ,
   ADDRESS VARCHAR(64) NULL ,
   COUNT INTEGER NULL ,
   "DATE" DATE NULL,
   PAY DECIMAL(18,2) NULL,
   CONSTRAINT PK_SIMPLES PRIMARY KEY (ID)
)
NOLOGGING 
NOCACHE 
NOPARALLEL;


/*==============================================================*/
/* Table : NULLABLE                                             */
/*==============================================================*/

CREATE TABLE NULLABLE (
   ID INTEGER NOT NULL,
   TESTBOOL SMALLINT NULL ,
   TESTBYTE SMALLINT NULL ,
   TESTCHAR CHAR(1) NULL ,
   TESTDATETIME DATE NULL ,
   TESTDECIMAL DECIMAL(9,2) NULL ,
   TESTDOUBLE FLOAT NULL ,
   TESTGUID VARCHAR2(36) NULL ,
   TESTINT16 SMALLINT NULL ,
   TESTINT32 INTEGER NULL ,
   TESTINT64 NUMBER NULL ,
   TESTSINGLE REAL NULL ,
   CONSTRAINT PK_NULLABLE PRIMARY KEY (ID)
)
NOLOGGING 
NOCACHE 
NOPARALLEL;