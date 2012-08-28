DROP database "IBatisNet";
DROP database "NHibernate";

-- User: "IBatisNet"

DROP USER "IBatisNet";

CREATE USER "IBatisNet" PASSWORD 'test'
  CREATEDB CREATEUSER
   VALID UNTIL 'infinity';

-- User: "NHibernate"
  
DROP USER "NHibernate";

CREATE USER "NHibernate" PASSWORD 'test'
  CREATEDB CREATEUSER
   VALID UNTIL 'infinity';


-- Database: "IBatisNet"

CREATE DATABASE "IBatisNet"
WITH ENCODING='UNICODE'
     OWNER="IBatisNet";

-- Database: "NHibernate"

CREATE DATABASE "NHibernate"
  WITH ENCODING='UNICODE'
       OWNER="NHibernate";




\connect "IBatisNet" "IBatisNet" using test;

-- DROP TABLE Accounts;

-- DROP TABLE Categories;

-- DROP TABLE Enumerations;

-- DROP TABLE LineItems;

-- DROP TABLE Orders;

-- DROP TABLE Others;

-- DROP TABLE Documents;



-- Sequence: S_Categories

-- DROP SEQUENCE S_Categories;

/*
CREATE SEQUENCE S_Categories
  INCREMENT 1
  MINVALUE 1
  MAXVALUE 9223372036854775807
  START 1
  CACHE 20;
ALTER TABLE S_Categories OWNER TO "IBatisNet";
*/

/*==============================================================*/
/* Table : Accounts                                             */
/*==============================================================*/
CREATE TABLE Accounts
(
   Account_Id int4 NOT NULL, 
   Account_FirstName varchar(32) NOT NULL, 
   Account_LastName varchar(32) NOT NULL, 
   Account_Email varchar(128),
   Account_Banner_Option varchar(255),
   Account_Cart_Option int4,   
   CONSTRAINT PK_Accounts PRIMARY KEY (Account_Id)   
) WITHOUT OIDS;
ALTER TABLE Accounts OWNER TO "IBatisNet";

/*==============================================================*/
/* Table : Categories                                           */
/*==============================================================*/
CREATE TABLE Categories
(
  Category_Id serial,
  Category_Name varchar(32),
  Category_Guid varchar(36),
  CONSTRAINT PK_Categories PRIMARY KEY (Category_Id)
) 
WITHOUT OIDS;
ALTER TABLE Categories OWNER TO "IBatisNet";

/*==============================================================*/
/* Table : Enumerations                                         */
/*==============================================================*/
CREATE TABLE Enumerations
(
  Enum_Id int4 NOT NULL,
  Enum_Day int4 NOT NULL,
  Enum_Color int4 NOT NULL,
  Enum_Month int4,
  CONSTRAINT PK_Enumerations PRIMARY KEY (Enum_Id)
) 
WITHOUT OIDS;
ALTER TABLE Enumerations OWNER TO "IBatisNet";

/*==============================================================*/
/* Table : LineItems                                            */
/*==============================================================*/
CREATE TABLE LineItems
(
  LineItem_Id int4 NOT NULL,
  Order_Id int4 NOT NULL,
  LineItem_Code varchar(32) NOT NULL,
  LineItem_Quantity int4 NOT NULL,
  LineItem_Price numeric(18,2),
  LineItem_Picture bytea,
  CONSTRAINT PK_LineItems PRIMARY KEY (LineItem_Id, Order_Id)
) 
WITHOUT OIDS;
ALTER TABLE LineItems OWNER TO "IBatisNet";

/*==============================================================*/
/* Table : Orders                                               */
/*==============================================================*/
CREATE TABLE Orders
(
  Order_Id int4 NOT NULL,
  Account_Id int4 NOT NULL,
  Order_Date timestamp,
  Order_CardType varchar(32),
  Order_CardNumber varchar(32),
  Order_CardExpiry varchar(32),
  Order_Street varchar(32),
  Order_City varchar(32),
  Order_Province varchar(32),
  Order_PostalCode varchar(32),
  Order_FavouriteLineItem int4,
  CONSTRAINT PK_Orders PRIMARY KEY (Order_Id)
) 
WITHOUT OIDS;
ALTER TABLE Orders OWNER TO "IBatisNet";

/*==============================================================*/
/* Table : Others                                               */
/*==============================================================*/
CREATE TABLE Others
(
  Other_Int int4,
  Other_Long int8,
  Other_Bit bool NOT NULL DEFAULT false,
  Other_String varchar(32) NOT NULL
) 
WITHOUT OIDS;
ALTER TABLE Others OWNER TO "IBatisNet";

/*==============================================================*/
/* Table : Documents                                            */
/*==============================================================*/
CREATE TABLE Documents
(
  Document_Id int4 NOT NULL,
  Document_Title varchar(32),
  Document_Type varchar(32),
  Document_PageNumber int4,
  Document_City varchar(32),
  CONSTRAINT PK_Documents PRIMARY KEY (Document_Id)
) 
WITHOUT OIDS;
ALTER TABLE Documents OWNER TO "IBatisNet";




\connect "NHibernate" "NHibernate" using test;

-- DROP TABLE Users;

/*==============================================================*/
/* Table : Users                                                */
/*==============================================================*/
CREATE TABLE Users
(
  LogonId varchar(20) NOT NULL DEFAULT '0'::character varying,
  Name varchar(40),
  Password varchar(20),
  EmailAddress varchar(40),
  LastLogon date,
  CONSTRAINT PK_Users PRIMARY KEY (LogonId)
) 
WITHOUT OIDS;
ALTER TABLE Users OWNER TO "NHibernate";
