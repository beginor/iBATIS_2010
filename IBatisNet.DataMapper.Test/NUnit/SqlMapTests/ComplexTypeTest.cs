
using System;
using System.Collections;
using System.Configuration;

using NUnit.Framework;

using IBatisNet.DataMapper.Test.Domain;

namespace IBatisNet.DataMapper.Test.NUnit.SqlMapTests
{
	/// <summary>
	/// Summary description for ComplexTypeTest.
	/// </summary>
	[TestFixture] 
	public class ComplexTypeTest : BaseTest
	{
		#region SetUp & TearDown

		/// <summary>
		/// SetUp
		/// </summary>
		[SetUp] 
		public void Init() 
		{
			InitScript( sqlMap.DataSource, ScriptDirectory + "account-init.sql" );
		}

		/// <summary>
		/// TearDown
		/// </summary>
		[TearDown] 
		public void Dispose()
		{ /* ... */ } 

		#endregion

		#region Complex type tests

		/// <summary>
		/// Complex type test
		/// </summary>
		[Test] 
		public void TestMapObjMap() 
		{
			Hashtable map = new Hashtable();
			Complex obj = new Complex();
			obj.Map = new Hashtable();
			obj.Map.Add("Id", 1);
			map.Add("obj", obj);
		    
			int id = (int)sqlMap.QueryForObject("ComplexMap", map);

			Assert.AreEqual(id, obj.Map["Id"]);
		}

		/// <summary>
		/// Complex type insert inline default null test
		/// </summary>
		[Test] 
		public void TestInsertMapObjMapAcctInlineDefaultNull() 
		{
			Hashtable map = new Hashtable();
			Account acct = NewAccount6();
			Complex obj = new Complex();
			obj.Map = new Hashtable();
			obj.Map.Add("acct", acct);
			map.Add("obj", obj);

			sqlMap.Insert("InsertComplexAccountViaInlineDefaultNull", map);

			Account account = (Account) sqlMap.QueryForObject("GetAccountNullableEmail", 6);

			AssertAccount6(account);
		}

		#endregion


	}
}
