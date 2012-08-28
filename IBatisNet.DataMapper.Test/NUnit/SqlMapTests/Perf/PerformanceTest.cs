using System;
using System.ComponentModel;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using IBatisNet.DataMapper.Test.Domain;

using NUnit.Framework;
using IBatisNet.DataMapper;
using IBatisNet.Common;
using CategoryAttribute = NUnit.Framework.CategoryAttribute;

namespace IBatisNet.DataMapper.Test.NUnit.SqlMapTests.Perf
{
    [TestFixture]
    [Category("Performance")]
    public class PerformanceTest : BaseTest
    {

		#region SetUp & TearDown

		/// <summary>
		/// SetUp
		/// </summary>
		[SetUp] 
		public void Init() 
		{
			InitScript( sqlMap.DataSource, ScriptDirectory + "simple-init.sql" );
		}

		/// <summary>
		/// TearDown
		/// </summary>
		[TearDown] 
		public void Dispose()
		{ /* ... */ } 

		#endregion

        #region DataMapper
        [Test]
        public void IbatisOnly()
        {
            for (int n = 2; n < 4000; n *= 2)
            {
                Simple[] simples = new Simple[n];
                object[] ids = new object[n];
                for (int i = 0; i < n; i++)
                {
                    simples[i] = new Simple();
                    simples[i].Init();
                    simples[i].Count = i;
                    simples[i].Id = i;
                }

                //Now do timings
				Timer timer = new Timer();
				GC.Collect();
				GC.WaitForPendingFinalizers();

                sqlMap.OpenConnection();
                timer.Start();
                Ibatis(simples, n, "h1");
				timer.Stop();
                double ibatis = 1000000 * (timer.Duration / (double)n);
                sqlMap.CloseConnection();

                sqlMap.OpenConnection();
				timer.Start();
                Ibatis(simples, n, "h2");
				timer.Stop();
                ibatis += 1000000 * (timer.Duration / (double)n);
                sqlMap.CloseConnection();

                sqlMap.OpenConnection();
				timer.Start();
                Ibatis(simples, n, "h2");
				timer.Stop();
                ibatis += 1000000 * (timer.Duration / (double)n);
                sqlMap.CloseConnection();

                System.Console.WriteLine("Objects: " + n + " - iBATIS DataMapper: " + ibatis.ToString("F3"));
            }
            System.GC.Collect();
        }

        private void Ibatis(Simple[] simples, int N, string runname)
        {
            sqlMap.BeginTransaction(false);

            for (int i = 0; i < N; i++)
            {
                sqlMap.Insert("InsertSimple", simples[i]);
            }

            for (int i = 0; i < N; i++)
            {
                simples[i].Name = "NH - " + i + N + runname + " - " + System.DateTime.Now.Ticks;
                sqlMap.Update("UpdateSimple", simples[i]);
            }

            for (int i = 0; i < N; i++)
            {
                sqlMap.Delete("DeleteSimple", simples[i].Id);
            }

            sqlMap.CommitTransaction(false);
        } 
        #endregion

        #region ADO.NET
        [Test]
        public void AdoNetOnly()
        {
            for (int n = 2; n < 4000; n *= 2)
            {
                Simple[] simples = new Simple[n];
                for (int i = 0; i < n; i++)
                {
                    simples[i] = new Simple();
                    simples[i].Init();
                    simples[i].Count = i;
                    simples[i].Id = i;
                }

                //Now do timings
				Timer timer = new Timer();

                IDbConnection _connection = sqlMap.DataSource.DbProvider.CreateConnection();
                _connection.ConnectionString = sqlMap.DataSource.ConnectionString;

                _connection.Open();

                timer.Start();
                DirectAdoNet(_connection, simples, n, "j1");
                timer.Stop(); 
                double adonet = 1000000 * (timer.Duration / (double)n);
                _connection.Close();

                _connection.Open();
                timer.Start();
                DirectAdoNet(_connection, simples, n, "j2");
				timer.Stop();
                adonet += 1000000 * (timer.Duration / (double)n);
                _connection.Close();

                _connection.Open();
                timer.Start();
                DirectAdoNet(_connection, simples, n, "j2");
				timer.Stop();
                adonet += 1000000 * (timer.Duration / (double)n);
                _connection.Close();

                System.Console.Out.WriteLine("Objects: " + n + " Direct ADO.NET: " + adonet.ToString("F3"));
            }
            System.GC.Collect();
        }

        private void DirectAdoNet(IDbConnection c, Simple[] simples, int N, string runname)
        {
            IDbCommand insert = InsertCommand();
            IDbCommand delete = DeleteCommand();
            IDbCommand select = SelectCommand();
            IDbCommand update = UpdateCommand();

            IDbTransaction t = c.BeginTransaction();

            insert.Connection = c;
            delete.Connection = c;
            select.Connection = c;
            update.Connection = c;

            insert.Transaction = t;
            delete.Transaction = t;
            select.Transaction = t;
            update.Transaction = t;

            insert.Prepare();
            delete.Prepare();
            select.Prepare();
            update.Prepare();

            for (int i = 0; i < N; i++)
            {
                ((IDbDataParameter)insert.Parameters[0]).Value = simples[i].Name;
                ((IDbDataParameter)insert.Parameters[1]).Value = simples[i].Address;
                ((IDbDataParameter)insert.Parameters[2]).Value = simples[i].Count;
                ((IDbDataParameter)insert.Parameters[3]).Value = simples[i].Date;
                ((IDbDataParameter)insert.Parameters[4]).Value = simples[i].Pay;
                ((IDbDataParameter)insert.Parameters[5]).Value = simples[i].Id;

                insert.ExecuteNonQuery();
            }

            for (int i = 0; i < N; i++)
            {
                ((IDbDataParameter)update.Parameters[0]).Value = "DR - " + i + N + runname + " - " + System.DateTime.Now.Ticks;
                ((IDbDataParameter)update.Parameters[1]).Value = simples[i].Address;
                ((IDbDataParameter)update.Parameters[2]).Value = simples[i].Count;
                ((IDbDataParameter)update.Parameters[3]).Value = simples[i].Date;
                ((IDbDataParameter)update.Parameters[4]).Value = simples[i].Pay;
                ((IDbDataParameter)update.Parameters[5]).Value = simples[i].Id;

                update.ExecuteNonQuery();
            }

            for (int i = 0; i < N; i++)
            {
                ((IDbDataParameter)delete.Parameters[0]).Value = simples[i].Id;
                delete.ExecuteNonQuery();
            }

            t.Commit();
        }

        private IDbCommand DeleteCommand()
        {
            string sql = "delete from Simples where id = ";
            sql += sqlMap.DataSource.DbProvider.FormatNameForSql("id");

            IDbCommand cmd = sqlMap.DataSource.DbProvider.CreateCommand();
            cmd.CommandText = sql;

            IDbDataParameter prm = cmd.CreateParameter();
            prm.ParameterName = sqlMap.DataSource.DbProvider.FormatNameForParameter("id");
            prm.DbType = DbType.Int32;
            cmd.Parameters.Add(prm);

            return cmd;
        }

        private IDbCommand InsertCommand()
        {
            string sql = "insert into Simples ( name, address, count, date, pay, id ) values (";
            for (int i = 0; i < 6; i++)
            {
                if (i > 0) sql += ", ";
                sql += sqlMap.DataSource.DbProvider.FormatNameForSql("param" + i.ToString());
            }

            sql += ")";

            IDbCommand cmd = sqlMap.DataSource.DbProvider.CreateCommand();
            cmd.CommandText = sql;
            AppendInsertUpdateParams(cmd);

            return cmd;
        }

        private IDbCommand SelectCommand()
        {
            string sql = "SELECT s.id, s.name, s.address, s.count, s.date, s.pay FROM Simples s";

            IDbCommand cmd = sqlMap.DataSource.DbProvider.CreateCommand();
            cmd.CommandText = sql;

            return cmd;
        }

        private IDbCommand UpdateCommand()
        {
            string sql = "update Simples set";
            sql += (" name = " + sqlMap.DataSource.DbProvider.FormatNameForSql("param0"));
            sql += (", address = " + sqlMap.DataSource.DbProvider.FormatNameForSql("param1"));
            sql += (", count = " + sqlMap.DataSource.DbProvider.FormatNameForSql("param2"));
            sql += (", date = " + sqlMap.DataSource.DbProvider.FormatNameForSql("param3"));
            sql += (", pay = " + sqlMap.DataSource.DbProvider.FormatNameForSql("param4"));
            sql += " where id = " + sqlMap.DataSource.DbProvider.FormatNameForSql("param5");

            IDbCommand cmd = sqlMap.DataSource.DbProvider.CreateCommand();
            cmd.CommandText = sql;

            AppendInsertUpdateParams(cmd);

            return cmd;
        }

        private void AppendInsertUpdateParams(IDbCommand cmd)
        {
            IDbDataParameter[] prm = new IDbDataParameter[6];
            for (int j = 0; j < 6; j++)
            {
                prm[j] = cmd.CreateParameter();
                prm[j].ParameterName = sqlMap.DataSource.DbProvider.FormatNameForParameter("param" + j.ToString());
                cmd.Parameters.Add(prm[j]);
            }

            int i = 0;
            prm[i].DbType = DbType.String;
            prm[i].Size = 255;
            i++;

            prm[i].DbType = DbType.String;
            prm[i].Size = 200;
            i++;

            prm[i].DbType = DbType.Int32;
            i++;

            prm[i].DbType = DbType.DateTime;
            i++;

            prm[i].DbType = DbType.Decimal;
            prm[i].Scale = 2;
            prm[i].Precision = 5;
            i++;

            prm[i].DbType = DbType.Int32;
            i++;

        } 
        #endregion

        [Test]
        public void Many()
        {
            double ibatis = 0;
            double adonet = 0;

            for (int n = 0; n < 5; n++)
            {
                Simple[] simples = new Simple[n];
                for (int i = 0; i < n; i++)
                {
                    simples[i] = new Simple();
                    simples[i].Init();
                    simples[i].Count = i;
                    simples[i].Id = i;
                }

                sqlMap.OpenConnection();
                Ibatis(simples, n, "h0");
                sqlMap.CloseConnection();

                IDbConnection _connection = sqlMap.DataSource.DbProvider.CreateConnection();
                _connection.ConnectionString = sqlMap.DataSource.ConnectionString;

                _connection.Open();
                DirectAdoNet(_connection, simples, n, "j0");
                _connection.Close();

                sqlMap.OpenConnection();
                Ibatis(simples, n, "h0");
                sqlMap.CloseConnection();

                _connection.Open();
                DirectAdoNet(_connection, simples, n, "j0");
                _connection.Close();

                // now do timings

                int loops = 30;
                Timer timer = new Timer();

                for (int runIndex = 1; runIndex < 4; runIndex++)
                {
					GC.Collect();
					GC.WaitForPendingFinalizers();

					timer.Start();
                    for (int i = 0; i < loops; i++)
                    {
                        sqlMap.OpenConnection();    
						Ibatis(simples, n, "h" + runIndex.ToString());
                         sqlMap.CloseConnection();
                    }
					timer.Stop();
					ibatis += 1000000 * (timer.Duration / (double)loops);

					GC.Collect();
					GC.WaitForPendingFinalizers();

                    timer.Start();
                    for (int i = 0; i < loops; i++)
                    {
                        _connection.Open();
                        DirectAdoNet(_connection, simples, n, "j" + runIndex.ToString());
                        _connection.Close();
                    }
					timer.Stop();
                    adonet += 1000000 * (timer.Duration / (double)loops);


                }
            }
            System.Console.Out.WriteLine("iBatis DataMapper : " + ibatis.ToString("F3") + " / Direct ADO.NET: " + adonet.ToString("F3") + " Ratio: " + ((ibatis / adonet)).ToString("F3"));

            System.GC.Collect();
        }

        [Test]
        public void Simultaneous()
        {
            double ibatis = 0;
            double adonet = 0;

            IDbConnection _connection = sqlMap.DataSource.DbProvider.CreateConnection();
            _connection.ConnectionString = sqlMap.DataSource.ConnectionString;

            for (int n = 2; n < 4000; n *= 2)
            {
                Simple[] simples = new Simple[n];
                for (int i = 0; i < n; i++)
                {
                    simples[i] = new Simple();
                    simples[i].Init();
                    simples[i].Count = i;
                    simples[i].Id = i;
                }

                sqlMap.OpenConnection();
                Ibatis(simples, n, "h0");
                sqlMap.CloseConnection();


                _connection.Open();
                DirectAdoNet(_connection, simples, n, "j0");
                _connection.Close();

                sqlMap.OpenConnection();
                Ibatis(simples, n, "h0");
                sqlMap.CloseConnection();

                _connection.Open();
                DirectAdoNet(_connection, simples, n, "j0");
                _connection.Close();

                //Now do timings
				Timer timer = new Timer();

				GC.Collect();
				GC.WaitForPendingFinalizers();

                sqlMap.OpenConnection();
                timer.Start();
                Ibatis(simples, n, "h1");
				timer.Stop();
                ibatis = 1000000 * (timer.Duration / (double)n);
                sqlMap.CloseConnection();

                _connection.Open();
                timer.Start();
                DirectAdoNet(_connection, simples, n, "j1");
				timer.Stop();
                adonet = 1000000 * (timer.Duration / (double)n);
                _connection.Close();

                sqlMap.OpenConnection();
                timer.Start();
                Ibatis(simples, n, "h2");
				timer.Stop();
                ibatis += 1000000 * (timer.Duration / (double)n);
                sqlMap.CloseConnection();

                _connection.Open();
                timer.Start();
                DirectAdoNet(_connection, simples, n, "j2");
				timer.Stop();
                adonet += 1000000 * (timer.Duration / (double)n);
                _connection.Close();

                sqlMap.OpenConnection();
                timer.Start();
                Ibatis(simples, n, "h2");
				timer.Stop();
                ibatis += 1000000 * (timer.Duration / (double)n);
                sqlMap.CloseConnection();

                _connection.Open();
                timer.Start();
                DirectAdoNet(_connection, simples, n, "j2");
				timer.Stop();
                adonet += 1000000 * (timer.Duration / (double)n);
                _connection.Close();
                System.Console.Out.WriteLine("Objects " + n + " iBATIS DataMapper : " + ibatis.ToString("F3") + " / Direct ADO.NET: " + adonet.ToString("F3") + " Ratio: " + ((ibatis / adonet)).ToString("F3"));
            }

            System.GC.Collect();
        }

		internal class Timer
		{
			[DllImport("Kernel32.dll")]
			private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

			[DllImport("Kernel32.dll")]
			private static extern bool QueryPerformanceFrequency(out long lpFrequency);

			private long startTime, stopTime;
			private long freq;

			public Timer()
			{
				startTime = 0;
				stopTime = 0;

				if (QueryPerformanceFrequency(out freq) == false)
				{
					throw new Win32Exception();
				}
			}

			public void Start()
			{
				Thread.Sleep(0);
				QueryPerformanceCounter(out startTime);
			}

			public void Stop()
			{
				QueryPerformanceCounter(out stopTime);
			}

			public double Duration
			{
				get { return (double) (stopTime - startTime)/(double) freq; }
			}
		}
    }
}
