drop table ENUMERATIONS;

create table ENUMERATIONS
(
   [ENUM_ID]                        long                            not null,
   [ENUM_DAY]                       long                            not null,
   [ENUM_COLOR]                     long                            not null,
   [ENUM_MONTH]                     long,
   constraint pkEnumerations
   primary key ([ENUM_ID])
);

INSERT INTO Enumerations VALUES(1, 1, 1, 128);
INSERT INTO Enumerations VALUES(2, 2, 2, 2048);
INSERT INTO Enumerations VALUES(3, 3, 4, 256);
INSERT INTO Enumerations VALUES(4, 4, 8, null);
