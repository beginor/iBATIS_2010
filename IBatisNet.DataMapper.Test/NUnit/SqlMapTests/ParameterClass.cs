using System;
using System.Collections;
using NUnit.Framework;

namespace IBatisNet.DataMapper.Test.NUnit.SqlMapTests
{
	/// <summary>
	/// Summary description for ResultClassTest.
	/// </summary>
	[TestFixture] 
	public class ParameterClass : BaseTest
	{
		/// <summary>
		///  Test passing DBNull.Value to a statement.
		/// </summary>
		[Test]
		public void TestDBNullValue()
		{
			int accountsWithNullEmail = (int)sqlMap.QueryForObject("GetCountOfAccountsWithNullEmail", null);

			Hashtable map = new Hashtable();
			map["DBNullValue"] = DBNull.Value;
			int rowsAffected = sqlMap.Update("UpdateNullEmailToDBNull", map);

			Assert.AreEqual(accountsWithNullEmail, rowsAffected);
		}
	}
}
