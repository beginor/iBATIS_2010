using System;
using System.Collections;
using System.Configuration;

using NUnit.Framework;

using IBatisNet.DataMapper; //<-- To access the definition of the deleagte RowDelegate
using IBatisNet.DataMapper.MappedStatements; //<-- To access the definition of the PageinatedList
using IBatisNet.Common;
using IBatisNet.Common.Exceptions;

using IBatisNet.DataMapper.Test;
using IBatisNet.DataMapper.Test.Domain;

namespace IBatisNet.DataMapper.Test.NUnit.SqlMapTests
{
	/// <summary>
	/// Summary description for InheritanceTest.
	/// </summary>
	[TestFixture] 
	public class InheritanceTest: BaseTest
	{

		#region SetUp & TearDown


		/// <summary>
		/// SetUp
		/// </summary>
		[SetUp] 
		public void Init() 
		{
            InitScript(sqlMap.DataSource, ScriptDirectory + "account-init.sql");
			InitScript( sqlMap.DataSource, ScriptDirectory + "documents-init.sql" );
		}


		/// <summary>
		/// TearDown
		/// </summary>
		[TearDown] 
		public void Dispose()
		{ /* ... */ } 

		#endregion

		#region Tests

		/// <summary>
		/// Test All document with no formula
		/// </summary>
		[Test] 
		public void GetAllDocument() 
		{
			IList list = sqlMap.QueryForList("GetAllDocument", null);

			Assert.AreEqual(6, list.Count);
			Book book = (Book) list[0];
			AssertBook(book, 1, "The World of Null-A", 55);

			book = (Book) list[1];
			AssertBook(book, 3, "Lord of the Rings", 3587);

			Document document = (Document) list[2];
			AssertDocument(document, 5, "Le Monde");

			document = (Document) list[3];
			AssertDocument(document, 6, "Foundation");

			Newspaper news = (Newspaper) list[4];
			AssertNewspaper(news, 2, "Le Progres de Lyon", "Lyon");

			document = (Document) list[5];
			AssertDocument(document, 4, "Le Canard enchaine");
		}

		/// <summary>
		/// Test All document in a typed collection
		/// </summary>
		[Test] 
		public void GetTypedCollection() 
		{
			DocumentCollection list = sqlMap.QueryForList("GetTypedCollection", null) as DocumentCollection;

			Assert.AreEqual(6, list.Count);

			Book book = (Book) list[0];
			AssertBook(book, 1, "The World of Null-A", 55);

			book = (Book) list[1];
			AssertBook(book, 3, "Lord of the Rings", 3587);

			Document document = list[2];
			AssertDocument(document, 5, "Le Monde");

			document = list[3];
			AssertDocument(document, 6, "Foundation");

			Newspaper news = (Newspaper) list[4];
			AssertNewspaper(news, 2, "Le Progres de Lyon", "Lyon");

			document = list[5];
			AssertDocument(document, 4, "Le Canard enchaine");
		}

		/// <summary>
		/// Test All document with Custom Type Handler
		/// </summary>
		[Test] 
		public void GetAllDocumentWithCustomTypeHandler() 
		{
			IList list = sqlMap.QueryForList("GetAllDocumentWithCustomTypeHandler", null);

			Assert.AreEqual(6, list.Count);
			Book book = (Book) list[0];
			AssertBook(book, 1, "The World of Null-A", 55);

			book = (Book) list[1];
			AssertBook(book, 3, "Lord of the Rings", 3587);

			Newspaper news = (Newspaper) list[2];
			AssertNewspaper(news, 5, "Le Monde", "Paris");

			book = (Book) list[3];
			AssertBook(book, 6, "Foundation", 557);

			news = (Newspaper) list[4];
			AssertNewspaper(news, 2, "Le Progres de Lyon", "Lyon");

			news = (Newspaper) list[5];
			AssertNewspaper(news, 4, "Le Canard enchaine", "Paris");
		}
	    
	    /// <summary>
		/// Test Inheritance On Result Property
		/// </summary>
        [Test]
        public void TestJIRA175()
	    {
            Account account = sqlMap.QueryForObject("JIRA175", 3) as Account;
            Assert.AreEqual(3, account.Id, "account.Id");
            Assert.AreEqual("William", account.FirstName, "account.FirstName");
	        
            Book book = account.Document as Book;
            Assert.IsNotNull(book);
            AssertBook(book, 3, "Lord of the Rings", 3587);
	    }
	    
		#endregion 

		void AssertDocument(Document document, int id, string title)
		{
			Assert.AreEqual(id, document.Id);
			Assert.AreEqual(title, document.Title);
		}

		void AssertBook(Book book, int id, string title, int pageNumber)
		{
			Assert.AreEqual(id, book.Id);
			Assert.AreEqual(title, book.Title);
			Assert.AreEqual(pageNumber, book.PageNumber);
		}

		void AssertNewspaper(Newspaper news, int id, string title, string city)
		{
			Assert.AreEqual(id, news.Id);
			Assert.AreEqual(title, news.Title);
			Assert.AreEqual(city, news.City);
		}

	}
}
