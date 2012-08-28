using System;
using System.Data;
using System.Data.SqlClient;

using System.Collections;

using NUnit.Framework;

using IBatisNet.DataMapper.Test.NUnit;
using IBatisNet.DataMapper.Test.Domain;

namespace IBatisNet.DataMapper.Test.NUnit.SqlMapTests.MSSQL
{
	/// <summary>
	/// Summary description for EmbedParameterTest.
	/// Test performance and sql injection using parameter values such as
	/// '; shutdown--
	/// or
	/// 1; shutdown--
	/// </summary>
	[TestFixture] 
	[Category("MSSQL")]
	[Category("EmbedParameter")]
	public class EmbedParameterTest : BaseTest
	{
		
		#region SetUp & TearDown
		/// <summary>
		/// SetUp
		/// </summary>
		[SetUp] 
		public void Init() 
		{
			InitSqlMap();
			//InitScript( sqlMap.DataSource, ScriptDirectory + "embed-param-test-init.sql", false );
		}

		/// <summary>
		/// TearDown
		/// </summary>
		[TearDown] 
		public void Dispose()
		{ /* ... */ } 

		#endregion

		#region Specific performance and sql injection tests for sql server

		/// <summary>
		/// Test GetManyRecordsBySequence.
		/// </summary>
		[Test] 
		public void TestQueryBySequence()
		{
			IList list = sqlMap.QueryForList("GetManyRecordsBySequence", 91);

			Assert.AreEqual(91, ((Sample) list[0]).SequenceId);
		}

		/// <summary>
		/// Test GetManyRecordsBySequenceWithIndex.
		/// </summary>
		[Test] 
		public void TestQueryBySequenceWithIndex()
		{
			IList list = sqlMap.QueryForList("GetManyRecordsBySequenceWithIndex", 91);

			Assert.AreEqual(91, ((Sample) list[0]).SequenceId);
		}

		/// <summary>
		/// Test GetManyRecordsByDistributed.
		/// </summary>
		[Test] 
		public void TestQueryByDistributed()
		{
			IList list = sqlMap.QueryForList("GetManyRecordsByDistributed", 91);

			Assert.AreEqual(91, ((Sample) list[0]).DistributedId);
		}

		/// <summary>
		/// Test GetManyRecordsByDistributedWithIndex.
		/// </summary>
		[Test] 
		public void TestQueryByDistributedWithIndex()
		{
			IList list = sqlMap.QueryForList("GetManyRecordsByDistributedWithIndex", 91);

			Assert.AreEqual(91, ((Sample) list[0]).DistributedId);
		}

		/// <summary>
		/// Test GetManyRecordsByFifth.
		/// </summary>
		[Test] 
		public void TestQueryByFifth()
		{
			IList list = sqlMap.QueryForList("GetManyRecordsByFifth", 30000);

			Assert.AreEqual(30000, ((Sample) list[0]).FifthId);
		}

		/// <summary>
		/// Test GetManyRecordsByFifthOrSequence.
		/// </summary>
		[Test] 
		public void TestQueryByFifthOrSequence()
		{
			Hashtable queryParams = new Hashtable();
			queryParams.Add("FifthId", 30000);
			queryParams.Add("SequenceId",  91);

			IList list = sqlMap.QueryForList("GetManyRecordsByFifthOrSequence", queryParams);

			Assert.IsNotNull(list);
			//Assert.AreEqual(30000, ((Sample) list[0]).FifthId);
			//Assert.AreEqual(91, ((Sample) list[0]).SequenceId);
		}

		/// <summary>
		/// Test GetManyRecordsByDates.
		/// </summary>
		[Test] 
		public void TestQueryByDates()
		{
			System.DateTime startDate = new System.DateTime(1999,1,1);
			System.DateTime endDate = new System.DateTime(1999,1,2);

			Hashtable queryParams = new Hashtable();
			queryParams.Add("StartDate", startDate);
			queryParams.Add("EndDate",  endDate);

			IList list = sqlMap.QueryForList("GetManyRecordsByDates", queryParams);

			Assert.IsNotNull(list);
		}

		/// <summary>
		/// Test GetManyRecordsByLikeChar.
		/// </summary>
		[Test] 
		public void TestQueryByLikeChar()
		{
			IList list = sqlMap.QueryForList("GetManyRecordsByLikeChar", "AAA");

			Assert.IsNotNull(list);
		}

		/// <summary>
		/// Test GetManyRecordsByChar.
		/// </summary>
		[Test] 
		public void TestInjectQueryByChar()
		{
			IList list = sqlMap.QueryForList("GetManyRecordsByChar", "'; shutdown--");

			Assert.IsNotNull(list);
		}
		
		#endregion
		
		[Test] 
		public void TestAdoPerformanceWithParameter()
		{
			string connectionString = sqlMap.DataSource.ConnectionString;

			// Embeded Parameters
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string commandText = @"SELECT 
					[Many_FirstID] AS FirstId, 
					[Many_SecondID] AS SecondId, 
					[Many_ThirdID] AS ThirdId, 
					[Many_FourthID] AS FourthId,  
					[Many_FifthID] AS FifthId,  
					[Many_SequenceID] AS SequenceId,  
					[Many_DistributedID] AS DistributedId, 
					[Many_SampleCharValue] AS SampleChar,  
					[Many_SampleDecimal] AS SampleDecimal, 
					[Many_SampleMoney] AS SampleMoney, 
					[Many_SampleDate] AS SampleDate, 
					[Many_SequenceDate] AS SequenceDate
				FROM 
					[ManyRecordsTest]
				WHERE 
					[Many_SampleDate] BETWEEN '1990/01/01' AND '1992/01/01'";

				connection.Open();

				using (SqlCommand command = new SqlCommand(commandText, connection))
				{
					command.CommandType = CommandType.Text;

					long startCount = DateTime.Now.Ticks;
					for(int i= 0; i<50; i++)
					{
						command.ExecuteNonQuery();
					}
					long endCount = DateTime.Now.Ticks;
					Console.Write("Embeded Parameters : ");
					Console.Write(endCount-startCount);
					Console.WriteLine();
				}
			}

			// Using Parameter
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string commandText = @"SELECT 
					[Many_FirstID] AS FirstId, 
					[Many_SecondID] AS SecondId, 
					[Many_ThirdID] AS ThirdId, 
					[Many_FourthID] AS FourthId,  
					[Many_FifthID] AS FifthId,  
					[Many_SequenceID] AS SequenceId,  
					[Many_DistributedID] AS DistributedId, 
					[Many_SampleCharValue] AS SampleChar,  
					[Many_SampleDecimal] AS SampleDecimal, 
					[Many_SampleMoney] AS SampleMoney, 
					[Many_SampleDate] AS SampleDate, 
					[Many_SequenceDate] AS SequenceDate
				FROM 
					[ManyRecordsTest]
				WHERE 
					[Many_SampleDate] BETWEEN @date1 AND @date2";

				connection.Open();

				using (SqlCommand command = new SqlCommand(commandText, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.Add ( "@date1", new DateTime(1990,1,1)) ;
					command.Parameters.Add ( "@date2", new DateTime(1992,1,1)) ;

					long startCount = DateTime.Now.Ticks;
					for(int i= 0; i<50; i++)
					{
						command.ExecuteNonQuery();
					}
					long endCount = DateTime.Now.Ticks;
					Console.Write("Parameter : ");
					Console.Write(endCount-startCount);
				}
			}

		}

	}
}
