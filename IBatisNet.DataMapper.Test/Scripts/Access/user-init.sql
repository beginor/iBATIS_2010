drop table USERS;

create table USERS
(
   [LOGONID]                      text(20)						not null default 0,
   [NAME]                         text(40)                      default null,
   [PASSWORD]                     text(20)						default null,
   [EMAILADDRESS]                 text(40)						default null,
   [LASTLOGON]                    datetime						default null,
   constraint pkUsers
   primary key ([LOGONID])
);
