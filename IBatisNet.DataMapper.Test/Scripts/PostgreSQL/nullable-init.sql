
DROP TABLE Nullable;

CREATE TABLE Nullable
(
  Id serial NOT NULL,
  TestBool bool,
  TestByte int4,
  TestChar char(1),
  TestDateTime timestamp(0),
  TestDecimal numeric(9,2),
  TestDouble float8,
  TestGuid char(16),
  TestInt16 int2,
  TestInt32 int4,
  TestInt64 int8,
  TestSingle float4,
  CONSTRAINT PK_Nullable PRIMARY KEY (Id)
) WITHOUT OIDS;
ALTER TABLE Nullable OWNER TO "IBatisNet";