using System;
using System.Collections;

using NUnit.Framework;

using IBatisNet.DataMapper.Test.NUnit;
using IBatisNet.DataMapper.Test.Domain;

namespace IBatisNet.DataMapper.Test.NUnit.SqlMapTests.PostgreSQL
{
	/// <summary>
	/// Summary description for StatementGenerate.
	/// </summary>
	[TestFixture] 
	[Category("PostgreSQL")]
	public class StatementGenerate : BaseTest
	{
		#region SetUp & TearDown
		
		/// <summary>
		/// SetUp
		/// </summary>
		[SetUp] 
		public void Init() 
		{
			InitScript( sqlMap.DataSource, ScriptDirectory + "category-init.sql" );
		}

		/// <summary>
		/// TearDown
		/// </summary>
		[TearDown] 
		public void Dispose()
		{ /* ... */ } 

		#endregion

		#region Specific statement test for PostgreSQL

		/// <summary>
		/// Test an select by key via generate statement.
		/// </summary>
		[Test] 
		public void TestSelectByPK()
		{
			Category category = new Category();
			category.Name = "toto";
			category.Guid = Guid.NewGuid();

			category.Id = (int)sqlMap.Insert("InsertCategoryGenerate", category);
			Assert.AreEqual(1, category.Id);

			Category categoryRead = null;
			categoryRead = (Category) sqlMap.QueryForObject("SelectByPKCategoryGenerate", category);

			Assert.AreEqual(category.Id, categoryRead.Id);
			Assert.AreEqual(category.Name, categoryRead.Name);
			Assert.AreEqual(category.Guid.ToString(), categoryRead.Guid.ToString());
		}

		/// <summary>
		/// Test an select all via generate statement.
		/// </summary>
		[Test] 
		public void TestSelectAll()
		{
			Category category = new Category();
			category.Name = "toto";
			category.Guid = Guid.NewGuid();

			int key = (int)sqlMap.Insert("InsertCategoryGenerate", category);
			Assert.AreEqual(1, key);

			category.Name = "toto";
			category.Guid = Guid.NewGuid();

			key = (int)sqlMap.Insert("InsertCategoryGenerate", category);
			Assert.AreEqual(2, key);

			IList categorieList = sqlMap.QueryForList("SelectAllCategoryGenerate",null) as IList;
			Assert.AreEqual(2, categorieList.Count);

		}

		/// <summary>
		/// Test an insert via generate statement.
		/// </summary>
		[Test] 
		public void TestInsert()
		{
			Category category = new Category();
			category.Name = "toto";
			category.Guid = Guid.Empty;

			int key = (int)sqlMap.Insert("InsertCategoryGenerate", category);
			Assert.AreEqual(1, key);
		}

		/// <summary>
		/// Test Update Category with Extended ParameterMap
		/// </summary>
		[Test] 
		public void TestUpdate()
		{
			Category category = new Category();
			category.Name = "Cat";
			category.Guid = Guid.NewGuid();

			int key = (int)sqlMap.Insert("InsertCategoryGenerate", category);
			category.Id = key;

			category.Name = "Dog";
			category.Guid = Guid.NewGuid();

			sqlMap.Update("UpdateCategoryGenerate", category);

			Category categoryRead = null;
			categoryRead = (Category) sqlMap.QueryForObject("GetCategory", key);

			Assert.AreEqual(category.Id, categoryRead.Id);
			Assert.AreEqual(category.Name, categoryRead.Name);
			Assert.AreEqual(category.Guid.ToString(), categoryRead.Guid.ToString());
		}
		
		/// <summary>
		/// Test an insert via generate statement.
		/// </summary>
		[Test] 
		public void TestDelete()
		{
			Category category = new Category();
			category.Name = "toto";
			category.Guid = Guid.NewGuid();

			int key = (int)sqlMap.Insert("InsertCategoryGenerate", category);
			category.Id = key;
			Assert.AreEqual(1, category.Id);
			
			sqlMap.Delete("DeleteCategoryGenerate", category);

			Category categoryRead = null;
			categoryRead = sqlMap.QueryForObject("GetCategory", key) as Category;

			Assert.IsNull(categoryRead);
		}
		#endregion
	}
}
