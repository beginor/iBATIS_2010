using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using IBatisNet.Common.Utilities;
using IBatisNet.DataMapper.Test.Domain;
using NUnit.Framework;

namespace IBatisNet.DataMapper.Test.NUnit.SqlMapTests
{
	/// <summary>
	/// Summary description for ParameterMapTest.
	/// </summary>
	[TestFixture] 
	public class ParameterMapTest : BaseTest
	{
		#region SetUp & TearDown

		/// <summary>
		/// SetUp
		/// </summary>
		[SetUp] 
		public void Init() 
		{
			InitScript( sqlMap.DataSource, ScriptDirectory+"account-init.sql" );
			InitScript( sqlMap.DataSource, ScriptDirectory+"account-procedure.sql", false );
			InitScript( sqlMap.DataSource, ScriptDirectory+"order-init.sql" );
			InitScript( sqlMap.DataSource, ScriptDirectory+"line-item-init.sql" );
			InitScript( sqlMap.DataSource, ScriptDirectory+"category-init.sql" );

		}

		/// <summary>
		/// TearDown
		/// </summary>
		[TearDown] 
		public void Dispose()
		{ /* ... */ } 

		#endregion

		#region Parameter map tests

		/// <summary>
		/// Test null replacement in ParameterMap property
		/// </summary>
		[Test] 
		public void TestNullValueReplacement()
		{
			Account account = NewAccount6();

			sqlMap.Insert("InsertAccountViaParameterMap", account);

			account = (Account) sqlMap.QueryForObject("GetAccountNullableEmail", 6);

			AssertAccount6(account);
		}


		/// <summary>
		/// Test Test Null Value Replacement Inline
		/// </summary>
		[Test] 
		public void TestNullValueReplacementInline() 
		{
			Account account = NewAccount6();

			sqlMap.Insert("InsertAccountViaInlineParameters", account);

			account = sqlMap.QueryForObject("GetAccountNullableEmail", 6) as Account;

			AssertAccount6(account);
		}

		/// <summary>
		/// Test Test Null Value Replacement Inline
		/// </summary>
		[Test] 
		public void TestSpecifiedType()
		{
			Account account = NewAccount6();
			account.EmailAddress = null;

			sqlMap.Insert("InsertAccountNullableEmail", account);

			account = sqlMap.QueryForObject("GetAccountNullableEmail", 6) as Account;

			AssertAccount6(account);
		}

		/// <summary>
		/// Test Test Null Value Replacement Inline
		/// </summary>
		[Test] 
		public void TestUnknownParameterClass()
		{
			Account account = NewAccount6();
			account.EmailAddress = null;

			sqlMap.Insert("InsertAccountUknownParameterClass", account);

			account = sqlMap.QueryForObject("GetAccountNullableEmail", 6) as Account;

			AssertAccount6(account);
		}

		/// <summary>
		/// Test null replacement in ParameterMap property
		/// for System.DateTime.MinValue
		/// </summary>
		[Test] 
		public void TestNullValueReplacementForDateTimeMinValue()
		{
			Account account = NewAccount6();

			sqlMap.Insert("InsertAccountViaParameterMap", account);

			Order order = new Order();
			order.Id = 99;
			order.CardExpiry = "09/11";
			order.Account = account;
			order.CardNumber = "154564656";
			order.CardType = "Visa";
			order.City = "Lyon";
			order.Date = DateTime.MinValue; //<-- null replacement
			order.PostalCode = "69004";
			order.Province = "Rhone";
			order.Street = "rue Durand";
 
			sqlMap.Insert("InsertOrderViaParameterMap", order);

			Order orderTest = (Order) sqlMap.QueryForObject("GetOrderLiteByColumnName", 99);

			Assert.AreEqual(order.City, orderTest.City);
		}

		/// <summary>
		/// Test null replacement in ParameterMap/Hahstable property
		/// for System.DateTime.MinValue
		/// </summary>
		[Test] 
		public void TestNullValueReplacementForDateTimeWithHashtable()
		{
			Account account = NewAccount6();

			sqlMap.Insert("InsertAccountViaParameterMap", account);

			Order order = new Order();
			order.Id = 99;
			order.CardExpiry = "09/11";
			order.Account = account;
			order.CardNumber = "154564656";
			order.CardType = "Visa";
			order.City = "Lyon";
			order.Date = DateTime.MinValue; //<-- null replacement
			order.PostalCode = "69004";
			order.Province = "Rhone";
			order.Street = "rue Durand";
 
			sqlMap.Insert("InsertOrderViaParameterMap", order);

			Hashtable orderTest = (Hashtable) sqlMap.QueryForObject("GetOrderByHashTable", 99);

			Assert.AreEqual(orderTest["Date"], DateTime.MinValue);
		}

		/// <summary>
		/// Test null replacement in ParameterMap property
		/// for Guid
		/// </summary>
		[Test] 
		public void TestNullValueReplacementForGuidValue()
		{
			Category category = new Category();
			category.Name = "Toto";
			category.Guid = Guid.Empty;

			int key = (int)sqlMap.Insert("InsertCategoryNull", category);

			Category categoryRead = null;
            categoryRead = (Category)sqlMap.QueryForObject("GetCategoryWithNullValueReplacementGuid", key);

			Assert.AreEqual(category.Name, categoryRead.Name);
			Assert.AreEqual(category.Guid.ToString(), categoryRead.Guid.ToString());
		}

		/// <summary>
		/// Test complex mapping Via hasTable 
		/// </summary>
		/// <example>
		/// 
		/// map.Add("Item", Item);
		/// map.Add("Order", Order);
		/// 
		/// <statement>
		/// ... #Item.prop1#...#Order.prop2#
		/// </statement>
		/// 
		/// </example>
		[Test]
		public void TestComplexMappingViaHasTable()
		{
			Hashtable param = new Hashtable();

			Account a = new Account();
			a.FirstName = "Joe";
			param.Add("Account",a);

			Order o = new Order();
			o.City = "Dalton";
			param.Add("Order", o);

			Account accountTest = (Account) sqlMap.QueryForObject("GetAccountComplexMapping", param);

			AssertAccount1(accountTest);
		}

		/// <summary>
		/// Test ByteArrayTypeHandler via Picture Property
		/// </summary>
		[Test]
		public void TestByteArrayTypeHandler()
		{
			Account account = NewAccount6();

			sqlMap.Insert("InsertAccountViaParameterMap", account);

			Order order = new Order();
			order.Id = 99;
			order.CardExpiry = "09/11";
			order.Account = account;
			order.CardNumber = "154564656";
			order.CardType = "Visa";
			order.City = "Lyon";
			order.Date = DateTime.MinValue; 
			order.PostalCode = "69004";
			order.Province = "Rhone";
			order.Street = "rue Durand";
 
			sqlMap.Insert("InsertOrderViaParameterMap", order);

			LineItem item = new LineItem();
			item.Id = 99;
			item.Code = "test";
			item.Price = -99.99m;
			item.Quantity = 99;
			item.Order = order;
			item.Picture = this.GetPicture();

			// Check insert
			sqlMap.Insert("InsertLineItemWithPicture", item);

			// select
			item = null;
			
			Hashtable param = new Hashtable();
			param.Add("LineItem_ID", 99);
			param.Add("Order_ID",  99);

			item = sqlMap.QueryForObject("GetSpecificLineItemWithPicture", param) as LineItem;

			Assert.IsNotNull( item );
			Assert.IsNotNull( item.Picture );
			Assert.AreEqual( GetSize(item.Picture), this.GetSize( this.GetPicture() ));
		}


        [Test]
        [Category("JIRA")]
        [Category("JIRA-253")]
        public void Null_byte_array_should_return_null()
        {
            Account account = NewAccount6();

            sqlMap.Insert("InsertAccountViaParameterMap", account);

            Order order = new Order();
            order.Id = 99;
            order.CardExpiry = "09/11";
            order.Account = account;
            order.CardNumber = "154564656";
            order.CardType = "Visa";
            order.City = "Lyon";
            order.Date = DateTime.MinValue;
            order.PostalCode = "69004";
            order.Province = "Rhone";
            order.Street = "rue Durand";

            sqlMap.Insert("InsertOrderViaParameterMap", order);

            LineItem item = new LineItem();
            item.Id = 99;
            item.Code = "test";
            item.Price = -99.99m;
            item.Quantity = 99;
            item.Order = order;
            item.Picture = null;

            // Check insert
            sqlMap.Insert("InsertLineItemWithPicture", item);

            // select
            item = null;

            Hashtable param = new Hashtable();
            param.Add("LineItem_ID", 99);
            param.Add("Order_ID", 99);

            item = sqlMap.QueryForObject("GetSpecificLineItemWithPicture", param) as LineItem;
            Assert.IsNotNull(item);
            Assert.IsNull(item.Picture);

        }


	    /// <summary>
		/// Test extend parameter map capacity
		/// (Support Requests 1043181)
		/// </summary>
		[Test] 
		public void TestInsertOrderViaExtendParameterMap()
		{
			Account account = NewAccount6();

			sqlMap.Insert("InsertAccountViaParameterMap", account);

			Order order = new Order();
			order.Id = 99;
			order.CardExpiry = "09/11";
			order.Account = account;
			order.CardNumber = "154564656";
			order.CardType = "Visa";
			order.City = "Lyon";
			order.Date = DateTime.MinValue; //<-- null replacement
			order.PostalCode = "69004";
			order.Province = "Rhone";
			order.Street = "rue Durand";
 
			sqlMap.Insert("InsertOrderViaExtendParameterMap", order);

			Order orderTest = (Order) sqlMap.QueryForObject("GetOrderLiteByColumnName", 99);

			Assert.AreEqual(order.City, orderTest.City);
		}

		#endregion

		#region Picture methods

		private Image GetPicture() 
		{
			Image _picture = null;

			// first try without path
			_picture = Image.FromFile( Path.Combine(Resources.ApplicationBase, "cool.jpg") );

			Assert.IsNotNull( _picture );
			return _picture;
		}

		private int GetSize( Image picture ) 
		{
			MemoryStream memoryStream = new MemoryStream();
			picture.Save (memoryStream, ImageFormat.Jpeg);
			return memoryStream.ToArray ().Length;
		}

		#endregion

	}
}
