DROP TABLE Accounts;

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

INSERT INTO Accounts VALUES(1,'Joe', 'Dalton', 'Joe.Dalton@somewhere.com', 'Oui', 200);
INSERT INTO Accounts VALUES(2,'Averel', 'Dalton', 'Averel.Dalton@somewhere.com', 'Oui', 200);
INSERT INTO Accounts VALUES(3,'William', 'Dalton', null, 'Non', 100);
INSERT INTO Accounts VALUES(4,'Jack', 'Dalton', 'Jack.Dalton@somewhere.com', 'Non', 100);
INSERT INTO Accounts VALUES(5,'Gilles', 'Bayon', null, 'Oui', 100);
