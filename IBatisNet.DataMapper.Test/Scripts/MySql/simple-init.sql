use IBatisNet;

drop table if exists Simples;

create table Simples
(
   ID int not null,
   Name varchar(64) NULL,
   Address varchar(64) NULL,
   Count int NULL,
   Date datetime NULL,
   Pay decimal(18,2) NULL,   
   primary key (ID)
) TYPE=INNODB;