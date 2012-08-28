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
	public class ResultClassTest : BaseTest
	{
		#region SetUp & TearDown

		/// <summary>
		/// SetUp
		/// </summary>
		[SetUp] 
		public void Init() 
		{
		}

		/// <summary>
		/// TearDown
		/// </summary>
		[TearDown] 
		public void Dispose()
		{ /* ... */ } 

		#endregion

		#region Specific statement test

		/// <summary>
		///  Test a boolean resultClass
		/// </summary>
		[Test]
		public void TestBoolean() 
		{
			bool bit = sqlMap.QueryForObject<bool>("GetBoolean", 1);

			Assert.AreEqual(true, bit);
		}

		/// <summary>
		///  Test a byte resultClass
		/// </summary>
		[Test] 
		public void TestByte() 
		{
			byte letter = sqlMap.QueryForObject<byte>("GetByte", 1);

			Assert.AreEqual(155, letter);
		}

		/// <summary>
		///  Test a char resultClass
		/// </summary>
		[Test] 
		public void TestChar() 
		{
			char letter = sqlMap.QueryForObject<char>("GetChar", 1);

			Assert.AreEqual('a', letter);
		}
        		
		/// <summary>
		///  Test a DateTime resultClass
		/// </summary>
		[Test] 
		public void TestDateTime() 
		{
			DateTime orderDate = sqlMap.QueryForObject<DateTime>("GetDate", 1);

			System.DateTime date = new DateTime(2003, 2, 15, 8, 15, 00);

			Assert.AreEqual(date.ToString(), orderDate.ToString());
		}

		/// <summary>
		///  Test a decimal resultClass
		/// </summary>
		[Test] 
		public void TestDecimal() 
		{
			decimal price = sqlMap.QueryForObject<decimal>("GetDecimal", 1);

			Assert.AreEqual((decimal)1.56, price);
		}

		/// <summary>
		///  Test a double resultClass
		/// </summary>
		[Test] 
		public void TestDouble() 
		{
			double price = sqlMap.QueryForObject<double>("GetDouble", 1);

			Assert.AreEqual(99.5f, price);
		}

		/// <summary>
		///  IBATISNET-25 Error applying ResultMap when using 'Guid' in resultClass
		/// </summary>
		[Test] 
		public void TestGuid() 
		{
			Guid newGuid = new Guid("CD5ABF17-4BBC-4C86-92F1-257735414CF4");

			Guid guid = sqlMap.QueryForObject<Guid>("GetGuid", 1);

			Assert.AreEqual(newGuid, guid);
		}

		/// <summary>
		///  Test a int16 resultClass
		/// </summary>
		[Test] 
		public void TestInt16() 
		{
			short integer = sqlMap.QueryForObject<short>("GetInt16", 1);

			Assert.AreEqual(32111, integer);
		}

		/// <summary>
		///  Test a int 32 resultClass
		/// </summary>
		[Test] 

		public void TestInt32() 
		{
			int integer = sqlMap.QueryForObject<int>("GetInt32", 1);

			Assert.AreEqual(999999, integer);
		}

		/// <summary>
		///  Test a int64 resultClass
		/// </summary>
		[Test] 
		public void TestInt64() 
		{
			long bigInt = sqlMap.QueryForObject<long>("GetInt64", 1);

			Assert.AreEqual(9223372036854775800, bigInt);
		}

		/// <summary>
		///  Test a single/float resultClass
		/// </summary>
		[Test] 
		public void TestSingle() 
		{
			float price = sqlMap.QueryForObject<float>("GetSingle", 1);

			Assert.AreEqual(92233.5, price);
		}

		/// <summary>
		///  Test a string resultClass
		/// </summary>
		[Test] 
		public void TestString() 
		{
			string cardType = sqlMap.QueryForObject<string>("GetString", 1);

			Assert.AreEqual("VISA", cardType);
		}

		
		#endregion
	}
}
#endif
