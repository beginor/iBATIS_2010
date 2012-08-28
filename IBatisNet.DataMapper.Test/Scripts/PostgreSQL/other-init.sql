DROP TABLE Others;
drop table A;
drop table B;
drop table C;
drop table D;
drop table E;
drop table F;

CREATE TABLE Others
(
  Other_Int int4,
  Other_Long int8,
  Other_Bit bool NOT NULL DEFAULT false,
  Other_String varchar(32) NOT NULL
) 
WITHOUT OIDS;
ALTER TABLE Others OWNER TO "IBatisNet";

CREATE TABLE F (
	ID varchar(50) NOT NULL ,
	F_Libelle varchar(50) NULL
) 
WITHOUT OIDS;
ALTER TABLE F OWNER TO "IBatisNet";

CREATE TABLE E (
	ID varchar(50) NOT NULL ,
	E_Libelle varchar(50) NULL 
) WITHOUT OIDS;
ALTER TABLE E OWNER TO "IBatisNet";

CREATE TABLE D (
	ID varchar(50) NOT NULL ,
	D_Libelle varchar(50) NULL 
) WITHOUT OIDS;
ALTER TABLE D OWNER TO "IBatisNet";

CREATE TABLE C (
	ID varchar(50) NOT NULL ,
	C_Libelle varchar(50) NULL 
) WITHOUT OIDS;
ALTER TABLE C OWNER TO "IBatisNet";

CREATE TABLE B (
	ID varchar(50) NOT NULL ,
	C_ID varchar(50) NULL ,
	D_ID varchar(50) NULL ,
	B_Libelle varchar(50) NULL
) WITHOUT OIDS;
ALTER TABLE B OWNER TO "IBatisNet";

CREATE TABLE A (
	Id varchar(50)  NOT NULL ,
	B_ID varchar(50)  NULL ,
	E_ID varchar(50)  NULL ,
	F_ID varchar(50)  NULL ,
	A_Libelle varchar(50)  NULL
) WITHOUT OIDS;
ALTER TABLE A OWNER TO "IBatisNet";

INSERT INTO Others VALUES(1, 8888888, false, 'Oui');
INSERT INTO Others VALUES(2, 9999999999, true, 'Non');

INSERT INTO F VALUES('f', 'fff');
INSERT INTO E VALUES('e', 'eee');
INSERT INTO D VALUES('d', 'ddd');
INSERT INTO C VALUES('c', 'ccc');
INSERT INTO B VALUES('b', 'c', null, 'bbb');
INSERT INTO A VALUES('a', 'b', 'e', null, 'aaa');