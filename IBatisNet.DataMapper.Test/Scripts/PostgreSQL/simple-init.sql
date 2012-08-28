drop table Simples;

create table Simples
(
   ID int4 not null,
   Name varchar(64) NULL,
   Address varchar(64) NULL,
   Count int4 NULL,
   Date timestamp NULL,
   Pay numeric(18,2) NULL,   
  CONSTRAINT PK_Simples PRIMARY KEY (ID)
) WITHOUT OIDS;
ALTER TABLE Simples OWNER TO "IBatisNet";