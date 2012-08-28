#if dotnet2
using System;

using NUnit.Framework;

using IBatisNet.DataMapper.Test.Domain;

namespace IBatisNet.DataMapper.Test.NUnit.SqlMapTests.Generics
{
    /// <summary>
    /// Summary description for ResultClassTest.
    /// </summary>
    [TestFixture]
    public class NullableTest : BaseTest
    {
        #region SetUp & TearDown

        /// <summary>
        /// SetUp
        /// </summary>
        [SetUp]
        public void Init()
        {
            InitScript(sqlMap.DataSource, ScriptDirectory + "Nullable-init.sql");
        }

        /// <summary>
        /// TearDown
        /// </summary>
        [TearDown]
        public void Dispose()
        {}

        #endregion

        #region NullableClass
        /// <summary>
        ///  Test a NullableClass resultClass
        /// </summary>
        [Test]
        public void TestNullableViaResultClass()
        {
            NullableClass clazz = new NullableClass();

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetClassNullable", 1);

            Assert.IsNotNull(clazz);
            Assert.AreEqual(1, clazz.Id);
            Assert.IsNull(clazz.TestBool);
            Assert.IsNull(clazz.TestByte);
            Assert.IsNull(clazz.TestChar);
            Assert.IsNull(clazz.TestDateTime);
            Assert.IsNull(clazz.TestDecimal);
            Assert.IsNull(clazz.TestDouble);
            Assert.IsNull(clazz.TestGuid);
            Assert.IsNull(clazz.TestInt16);
            Assert.IsNull(clazz.TestInt32);
            Assert.IsNull(clazz.TestInt64);
            Assert.IsNull(clazz.TestSingle);
        }

        /// <summary>
        ///  Test a NullableClass resultClass
        /// </summary>
        [Test]
        public void TestNullableViaResultClass2()
        {
            NullableClass clazz = new NullableClass();
            clazz.TestBool = true;
            clazz.TestByte = 155;
            clazz.TestChar = 'a';
            DateTime? date = new DateTime?(DateTime.Now);
            clazz.TestDateTime = date;
            clazz.TestDecimal = 99.53M;
            clazz.TestDouble = 99.5125;
            Guid? guid = new Guid?(Guid.NewGuid());
            clazz.TestGuid = guid;
            clazz.TestInt16 = 45;
            clazz.TestInt32 = 99;
            clazz.TestInt64 = 1234567890123456789;
            clazz.TestSingle = 4578.46445454112f;

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetClassNullable", 1);

            Assert.IsNotNull(clazz);
            Assert.AreEqual(1, clazz.Id);
            Assert.IsTrue(clazz.TestBool.Value);
            Assert.AreEqual(155, clazz.TestByte);
            Assert.AreEqual('a', clazz.TestChar);
            Assert.AreEqual(date.Value.ToString(), clazz.TestDateTime.Value.ToString());
            Assert.AreEqual(99.53M, clazz.TestDecimal);
            Assert.AreEqual(99.5125, clazz.TestDouble);
            Assert.AreEqual(guid, clazz.TestGuid);
            Assert.AreEqual(45, clazz.TestInt16);
            Assert.AreEqual(99, clazz.TestInt32);
            Assert.AreEqual(1234567890123456789, clazz.TestInt64);
            Assert.AreEqual(4578.46445454112f, clazz.TestSingle);
        } 
        #endregion

        #region bool

        /// <summary>
        /// Test nullable bool
        /// </summary>
        [Test]
        public void TestNullableBool()
        {
            NullableClass clazz = new NullableClass();

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.IsNotNull(clazz);
            Assert.AreEqual(1, clazz.Id);
            Assert.IsNull(clazz.TestBool);
        }

        /// <summary>
        /// Test not nullable bool
        /// </summary>
        [Test]
        public void TestNotNullableBool()
        {
            NullableClass clazz = new NullableClass();
            clazz.TestBool = false;

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.IsNotNull(clazz);
            Assert.AreEqual(1, clazz.Id);
            Assert.AreEqual(false, clazz.TestBool);
        } 
        #endregion

        #region byte
        /// <summary>
        /// Test nullable byte
        /// </summary>
        [Test]
        public void TestNullableByte()
        {
            NullableClass clazz = new NullableClass();

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.IsNotNull(clazz);
            Assert.AreEqual(1, clazz.Id);
            Assert.IsNull(clazz.TestByte);
        }

        /// <summary>
        /// Test not nullable byte
        /// </summary>
        [Test]
        public void TestNotNullableByte()
        {
            NullableClass clazz = new NullableClass();
            clazz.TestByte = 155;

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.IsNotNull(clazz);
            Assert.AreEqual(1, clazz.Id);
            Assert.AreEqual(155, clazz.TestByte);
        } 
        #endregion

        #region char
        /// <summary>
        /// Test nullable char
        /// </summary>
        [Test]
        public void TestNullableChar()
        {
            NullableClass clazz = new NullableClass();

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.IsNotNull(clazz);
            Assert.AreEqual(1, clazz.Id);
            Assert.IsNull(clazz.TestChar);
        }

        /// <summary>
        /// Test not nullable char
        /// </summary>
        [Test]
        public void TestNotNullableChar()
        {
            NullableClass clazz = new NullableClass();
            clazz.TestChar = 'a';

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.IsNotNull(clazz);
            Assert.AreEqual(1, clazz.Id);
            Assert.AreEqual('a', clazz.TestChar);
        } 
        #endregion

        #region datetime
        /// <summary>
        /// Test nullable datetime
        /// </summary>
        [Test]
        public void TestNullableDateTime()
        {
            NullableClass clazz = new NullableClass();

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.IsNotNull(clazz);
            Assert.AreEqual(1, clazz.Id);
            Assert.IsNull(clazz.TestDateTime);
        }

        /// <summary>
        /// Test not nullable datetime
        /// </summary>
        [Test]
        public void TestNotNullableDateTime()
        {
            NullableClass clazz = new NullableClass();
            DateTime? date = new DateTime?(DateTime.Now);
            clazz.TestDateTime = date;

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.IsNotNull(clazz);
            Assert.AreEqual(1, clazz.Id);
            Assert.AreEqual(date.Value.ToString(), clazz.TestDateTime.Value.ToString());
        }
        #endregion

        #region decimal
        /// <summary>
        /// Test nullable decimal
        /// </summary>
        [Test]
        public void TestNullableDecimal()
        {
            NullableClass clazz = new NullableClass();

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.IsNotNull(clazz);
            Assert.AreEqual(clazz.Id, 1);
            Assert.IsNull(clazz.TestDecimal);
        }

        /// <summary>
        /// Test not nullable decimal
        /// </summary>
        [Test]
        public void TestNotNullableDecimal()
        {
            NullableClass clazz = new NullableClass();
            clazz.TestDecimal = 99.53M;

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.IsNotNull(clazz);
            Assert.AreEqual(1, clazz.Id);
            Assert.AreEqual(99.53M, clazz.TestDecimal);
        }
        #endregion

        #region Double
        /// <summary>
        /// Test nullable Double
        /// </summary>
        [Test]
        public void TestNullableDouble()
        {
            NullableClass clazz = new NullableClass();

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.IsNotNull(clazz);
            Assert.AreEqual(clazz.Id, 1);
            Assert.IsNull(clazz.TestDouble);
        }

        /// <summary>
        /// Test not nullable Double
        /// </summary>
        [Test]
        public void TestNotNullableDouble()
        {
            NullableClass clazz = new NullableClass();
            clazz.TestDouble = 99.5125;

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.IsNotNull(clazz);
            Assert.AreEqual(1, clazz.Id);
            Assert.AreEqual(99.5125, clazz.TestDouble);
        }
        #endregion

        #region Guid
        /// <summary>
        /// Test nullable Guid
        /// </summary>
        [Test]
        public void TestNullableGuid()
        {
            NullableClass clazz = new NullableClass();

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.IsNotNull(clazz);
            Assert.AreEqual(clazz.Id, 1);
            Assert.IsNull(clazz.TestGuid);
        }

        /// <summary>
        /// Test not nullable Guid
        /// </summary>
        [Test]
        public void TestNotNullableGuid()
        {
            NullableClass clazz = new NullableClass();
            Guid? guid = new Guid?(Guid.NewGuid());
            clazz.TestGuid = guid;

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.IsNotNull(clazz);
            Assert.AreEqual(1, clazz.Id);
            Assert.AreEqual(guid, clazz.TestGuid);
        }
        #endregion

        #region Int16
        /// <summary>
        /// Test nullable Int16
        /// </summary>
        [Test]
        public void TestNullableInt16()
        {
            NullableClass clazz = new NullableClass();

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.IsNotNull(clazz);
            Assert.AreEqual(clazz.Id, 1);
            Assert.IsNull(clazz.TestInt16);
        }

        /// <summary>
        /// Test not nullable Int16
        /// </summary>
        [Test]
        public void TestNotNullableInt16()
        {
            NullableClass clazz = new NullableClass();
            clazz.TestInt16 = 45;

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.IsNotNull(clazz);
            Assert.AreEqual(1, clazz.Id);
            Assert.AreEqual(45, clazz.TestInt16);
        }
        #endregion

        #region int 32
        /// <summary>
        /// Test nullable int32
        /// </summary>
        [Test]
        public void TestNullableInt32()
        {
            NullableClass clazz = new NullableClass();

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.IsNotNull(clazz);
            Assert.AreEqual(clazz.Id, 1);
            Assert.IsNull(clazz.TestInt32);
        }

        /// <summary>
        /// Test not nullable int32
        /// </summary>
        [Test]
        public void TestNotNullableInt32()
        {
            NullableClass clazz = new NullableClass();
            clazz.TestInt32 = 99;

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.IsNotNull(clazz);
            Assert.AreEqual(1, clazz.Id);
            Assert.AreEqual(99, clazz.TestInt32);
        } 
        #endregion

        #region Int64
        /// <summary>
        /// Test nullable Int64
        /// </summary>
        [Test]
        public void TestNullableInt64()
        {
            NullableClass clazz = new NullableClass();

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.IsNotNull(clazz);
            Assert.AreEqual(clazz.Id, 1);
            Assert.IsNull(clazz.TestInt64);
        }

        /// <summary>
        /// Test not nullable Int64
        /// </summary>
        [Test]
        public void TestNotNullableInt64()
        {
            NullableClass clazz = new NullableClass();
            clazz.TestInt64 = 1234567890123456789;

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.IsNotNull(clazz);
            Assert.AreEqual(1, clazz.Id);
            Assert.AreEqual(1234567890123456789, clazz.TestInt64);
        }
        #endregion

        #region Single
        /// <summary>
        /// Test nullable Single
        /// </summary>
        [Test]
        public void TestNullableSingle()
        {
            NullableClass clazz = new NullableClass();

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.IsNotNull(clazz);
            Assert.AreEqual(clazz.Id, 1);
            Assert.IsNull(clazz.TestSingle);
        }

        /// <summary>
        /// Test not nullable Single
        /// </summary>
        [Test]
        public void TestNotNullableSingle()
        {
            NullableClass clazz = new NullableClass();
            clazz.TestSingle = 4578.46445454112f;

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.IsNotNull(clazz);
            Assert.AreEqual(1, clazz.Id);
            Assert.AreEqual(4578.46445454112f, clazz.TestSingle);
        }
        #endregion

        #region timespan
        
        [Test]
        public void TestNullableTimeSpan()
        {
            NullableClass clazz = new NullableClass();
            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.IsNotNull(clazz);
            Assert.AreEqual(1, clazz.Id);
            Assert.IsNull(clazz.TestTimeSpan);
        }

        /// <summary>
        /// Test not nullable timespan
        /// </summary>
        [Test]
        public void TestNotNullableTimeSpan()
        {
            NullableClass clazz = new NullableClass();
            TimeSpan? span = new TimeSpan?(new TimeSpan(1, 2, 3, 4, 5));
            clazz.TestTimeSpan = span;

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.IsNotNull(clazz);
            Assert.AreEqual(1, clazz.Id);
            Assert.AreEqual(span.Value.ToString(), clazz.TestTimeSpan.Value.ToString());
        }

        #endregion
    }
}
#endif