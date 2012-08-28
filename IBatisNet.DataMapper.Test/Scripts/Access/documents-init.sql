drop table DOCUMENTS;

create table DOCUMENTS
(
   [DOCUMENT_ID]                     long                            not null,
   [DOCUMENT_TITLE]                  text(32),
   [DOCUMENT_TYPE]                   text(32),
   [DOCUMENT_PAGENUMBER]             long,
   [DOCUMENT_CITY]                   text(32),
   constraint pkAccounts
   primary key ([DOCUMENT_ID])
);

INSERT INTO Documents VALUES (1, 'The World of Null-A', 'Book', 55, null);
INSERT INTO Documents VALUES (2, 'Le Progres de Lyon', 'Newspaper', null , 'Lyon');
INSERT INTO Documents VALUES (3, 'Lord of the Rings', 'Book', 3587, null);
INSERT INTO Documents VALUES (4, 'Le Canard enchaine', 'Tabloid', null , 'Paris');
INSERT INTO Documents VALUES (5, 'Le Monde', 'Broadsheet', null , 'Paris');
INSERT INTO Documents VALUES (6, 'Foundation', 'Monograph', 557, null);
