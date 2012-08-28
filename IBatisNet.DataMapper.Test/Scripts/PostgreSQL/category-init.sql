DROP TABLE Categories;

CREATE TABLE Categories
(
  Category_Id serial,
  Category_Name varchar(32),
  Category_Guid varchar(36),
  CONSTRAINT PK_Categories PRIMARY KEY (Category_Id)
) 
WITHOUT OIDS;
ALTER TABLE Categories OWNER TO "IBatisNet";
