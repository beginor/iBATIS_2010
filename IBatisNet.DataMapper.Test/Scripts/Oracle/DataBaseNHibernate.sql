DROP TABLE USERS;

/*==============================================================*/
/* Table : USERS                                                */
/*==============================================================*/

CREATE TABLE USERS (
  LOGONID 	 	   NVARCHAR2(20) 				DEFAULT '0',
  NAME 			   NVARCHAR2(40) 				DEFAULT NULL,
  PASSWORD 		   NVARCHAR2(20) 				DEFAULT NULL,
  EMAILADDRESS 	   NVARCHAR2(40) 				DEFAULT NULL,
  LASTLOGON 	   DATE 					DEFAULT NULL,
  CONSTRAINT PK_USERS PRIMARY KEY (LOGONID)
)
NOLOGGING
NOCACHE
NOPARALLEL;