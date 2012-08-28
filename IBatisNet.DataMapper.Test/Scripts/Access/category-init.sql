drop table CATEGORIES;

create table CATEGORIES
(
   [CATEGORY_ID]                    counter                     not null,
   [CATEGORY_NAME]                  text(32),
   [CATEGORY_GUID]                  guid,
   constraint pkCategories
   primary key ([CATEGORY_ID])
);
