drop table ACCOUNTS;

create table ACCOUNTS
(
   [ACCOUNT_ID]                     long                            not null,
   [ACCOUNT_FIRSTNAME]              text(32)                    not null,
   [ACCOUNT_LASTNAME]               text(32)                    not null,
   [ACCOUNT_EMAIL]                  text(128),
   constraint pkAccounts
   primary key ([ACCOUNT_ID])
);

INSERT INTO Accounts VALUES(1,'Joe', 'Dalton', 'Joe.Dalton@somewhere.com');
INSERT INTO Accounts VALUES(2,'Averel', 'Dalton', 'Averel.Dalton@somewhere.com');
INSERT INTO Accounts VALUES(3,'William', 'Dalton', null);
INSERT INTO Accounts VALUES(4,'Jack', 'Dalton', 'Jack.Dalton@somewhere.com');
INSERT INTO Accounts VALUES(5,'Gilles', 'Bayon', null);