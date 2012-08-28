DROP TABLE Users;

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
