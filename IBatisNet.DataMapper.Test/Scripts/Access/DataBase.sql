/* Certain queries must be run through ADO since Access does not
   recognize some SQL DDL keywords/data types (such as DECIMAL
   or DEFAULT) through the Access SQL Query window */ 

drop table ACCOUNTS;

drop table CATEGORIES;

drop table ENUMERATIONS;

drop table LINEITEMS;

drop table ORDERS;

drop table OTHERS;

create table ACCOUNTS
(
   [ACCOUNT_ID]                     long                        not null,
   [ACCOUNT_FIRSTNAME]              text(32)                    not null,
   [ACCOUNT_LASTNAME]               text(32)                    not null,
   [ACCOUNT_EMAIL]                  text(128),
   constraint pkAccounts
   primary key ([ACCOUNT_ID])
);

create table CATEGORIES
(
   [CATEGORY_ID]                    counter                     not null,
   [CATEGORY_NAME]                  text(32),
   [CATEGORY_GUID]                  guid,
   constraint pkCategories
   primary key ([CATEGORY_ID])
);

create table ENUMERATIONS
(
   [ENUM_ID]                        long                            not null,
   [ENUM_DAY]                       long                            not null,
   [ENUM_COLOR]                     long                            not null,
   [ENUM_MONTH]                     long,
   constraint pkEnumerations
   primary key ([ENUM_ID])
);

create table LINEITEMS
(
   [LINEITEM_ID]                    long                            not null,
   [ORDER_ID]                       long                            not null,
   [LINEITEM_CODE]                  text(32)                        not null,
   [LINEITEM_QUANTITY]              long                            not null,
   [LINEITEM_PRICE]                 decimal(18,2),
   [LINEITEM_PICTURE]               longbinary,
   constraint pkLineItems
   primary key ([ORDER_ID], [LINEITEM_ID])
);

create table ORDERS
(
   [ORDER_ID]                       long                            not null,
   [ACCOUNT_ID]                     long                            not null,
   [ORDER_DATE]                     datetime,
   [ORDER_CARDTYPE]                 text(32),
   [ORDER_CARDNUMBER]               text(32),
   [ORDER_CARDEXPIRY]               text(32),
   [ORDER_STREET]                   text(32),
   [ORDER_CITY]                     text(32),
   [ORDER_PROVINCE]                 text(32),
   [ORDER_POSTALCODE]               text(32),
   [ORDER_FAVOURITELINEITEM]        long,
   constraint pkOrders
   primary key ([ORDER_ID])
);

create table OTHERS
(
   OTHER_INT                       long,
   OTHER_LONG                      decimal
);

