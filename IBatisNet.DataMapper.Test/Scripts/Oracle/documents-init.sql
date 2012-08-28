DROP TABLE DOCUMENTS CASCADE CONSTRAINTS;

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

INSERT INTO DOCUMENTS VALUES (1, 'The World of Null-A', 'Book', 55, null);
INSERT INTO DOCUMENTS VALUES (2, 'Le Progres de Lyon', 'Newspaper', null , 'Lyon');
INSERT INTO DOCUMENTS VALUES (3, 'Lord of the Rings', 'Book', 3587, null);
INSERT INTO DOCUMENTS VALUES (4, 'Le Canard enchaine', 'Tabloid', null , 'Paris');
INSERT INTO DOCUMENTS VALUES (5, 'Le Monde', 'Broadsheet', null , 'Paris');
INSERT INTO DOCUMENTS VALUES (6, 'Foundation', 'Monograph', 557, null);
