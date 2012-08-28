DROP TABLE ORDERS CASCADE CONSTRAINTS;

CREATE TABLE ORDERS  (
   ORDER_ID             INTEGER                          NOT NULL,
   ACCOUNT_ID           INTEGER                          NULL,
   ORDER_DATE           DATE,
   ORDER_CARDTYPE       VARCHAR2(32),
   ORDER_CARDNUMBER     VARCHAR2(32),
   ORDER_CARDEXPIRY     VARCHAR2(32),
   ORDER_STREET         VARCHAR2(32),
   ORDER_CITY           VARCHAR2(32),
   ORDER_PROVINCE       VARCHAR2(32),
   ORDER_POSTALCODE     VARCHAR2(32),
   ORDER_FAVOURITELINEITEM INTEGER,
   CONSTRAINT PK_ORDERS PRIMARY KEY (ORDER_ID)
)
NOLOGGING 
NOCACHE 
NOPARALLEL;

INSERT INTO ORDERS VALUES (1, 1, TO_DATE('2003-02-15 8:15:00','yyyy-mm-dd hh:mi:ss'), 'VISA', '999999999999', '05/03', '11 This Street', 'Victoria', 'BC', 'C4B 4F4',2);
INSERT INTO ORDERS VALUES (2, 4, TO_DATE('2003-02-15 8:15:00','yyyy-mm-dd hh:mi:ss'), 'MC', '888888888888', '06/03', '222 That Street', 'Edmonton', 'AB', 'X4K 5Y4',1);
INSERT INTO ORDERS VALUES (3, 3, TO_DATE('2003-02-15 8:15:00','yyyy-mm-dd hh:mi:ss'), 'AMEX', '777777777777', '07/03', '333 Other Street', 'Regina', 'SK', 'Z4U 6Y4',2);
INSERT INTO ORDERS VALUES (4, 2, TO_DATE('2003-02-15 8:15:00','yyyy-mm-dd hh:mi:ss'), 'MC', '666666666666', '08/03', '444 His Street', 'Toronto', 'ON', 'K4U 3S4',1);
INSERT INTO ORDERS VALUES (5, 5, TO_DATE('2003-02-15 8:15:00','yyyy-mm-dd hh:mi:ss'), 'VISA', '555555555555', '09/03', '555 Her Street', 'Calgary', 'AB', 'J4J 7S4',2);
INSERT INTO ORDERS VALUES (6, 5, TO_DATE('2003-02-15 8:15:00','yyyy-mm-dd hh:mi:ss'), 'VISA', '999999999999', '10/03', '6 Their Street', 'Victoria', 'BC', 'T4H 9G4',1);
INSERT INTO ORDERS VALUES (7, 4, TO_DATE('2003-02-15 8:15:00','yyyy-mm-dd hh:mi:ss'), 'MC', '888888888888', '11/03', '77 Lucky Street', 'Edmonton', 'AB', 'R4A 0Z4',2);
INSERT INTO ORDERS VALUES (8, 3, TO_DATE('2003-02-15 8:15:00','yyyy-mm-dd hh:mi:ss'), 'AMEX', '777777777777', '12/03', '888 Our Street', 'Regina', 'SK', 'S4S 7G4',1);
INSERT INTO ORDERS VALUES (9, 2, TO_DATE('2003-02-15 8:15:00','yyyy-mm-dd hh:mi:ss'), 'MC', '666666666666', '01/04', '999 Your Street', 'Toronto', 'ON', 'G4D 9F4',2);
INSERT INTO ORDERS VALUES (10, 1, TO_DATE('2003-02-15 8:15:00','yyyy-mm-dd hh:mi:ss'), 'VISA', '555555555555', '02/04', '99 Some Street', 'Calgary', 'AB', 'W4G 7A4',1);
INSERT INTO ORDERS VALUES (11, null, TO_DATE('2003-02-15 8:15:00','yyyy-mm-dd hh:mi:ss'), 'VISA', '555555555555', '02/04', 'Null order', 'Calgary', 'ZZ', 'XXX YYY',1);
