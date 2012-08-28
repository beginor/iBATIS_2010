drop table OTHERS;

create table OTHERS
(
   OTHER_INT long,
   OTHER_LONG decimal,
   OTHER_BIT YESNO,
   OTHER_STRING text(32)
);

INSERT INTO Others VALUES(1, 8888888, false, 'Oui');
INSERT INTO Others VALUES(2, 9999999999,true, 'Non');
