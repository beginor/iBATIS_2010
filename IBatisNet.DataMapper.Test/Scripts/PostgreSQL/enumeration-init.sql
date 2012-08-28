DROP TABLE Enumerations;

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

INSERT INTO Enumerations VALUES(1, 1, 1, 128);
INSERT INTO Enumerations VALUES(2, 2, 2, 2048);
INSERT INTO Enumerations VALUES(3, 3, 4, 256);
INSERT INTO Enumerations VALUES(4, 4, 8, null);
