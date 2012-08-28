
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 638571 $
 * $Date: 2008-03-18 15:11:57 -0600 (Tue, 18 Mar 2008) $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2004 - Gilles Bayon
 *  
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 ********************************************************************************/
#endregion

#region Imports
using System;
using System.EnterpriseServices;
using System.Runtime.Remoting.Messaging;
using IBatisNet.Common.Logging;

#endregion

namespace IBatisNet.Common.Transaction
{
	/// <summary>
	/// Simple interface to COM+ transactions through Enterprise Service. 
	/// Makes a code block transactional à la Indigo (evolution will be easier, it's the same API)
	/// It's important to make sure that each instance 
	/// of this class gets Close()'d. 
	/// Easiest way to do that is with the using statement in C#.
	/// </summary>
	/// <remarks>
	/// Don't support nested transaction scope with different transaction options.
	///  
	/// System.EnterpriseServices.ServiceDomain is available only on 
	/// - XP SP2 (or higher) 
	/// - Windows Server 2003 
	/// - XP SP1 + Hotfix 828741 
	/// and only in .Net 1.1.
	/// It CAN'T be used on Windows 2000.
	/// 
	/// http://support.microsoft.com/default.aspx/kb/319177/EN-US/
	/// </remarks>
	/// <example>
	/// using (TransactionScope tx = new TransactionScope())
	///	{
	///		
	///		// Open connection to database 1	
	///		// Transaction will be automatically enlist into it
	///		// Execute update in database 1
	///		// Open connection to database 2
	///		// Transaction will be automatically enlist into it
	///		// Execute update in database 2
	///
	///		// the following code will be executed only if no exception
	///		// occured in the above code; since we got here ok, let's vote for commit;
	///		tx.Completed(); 
	///	}
	/// when “using” call Dispose on the transaction scope at the end
	/// of the “using” code block, the "ambient" transaction will be commited only and only if
	/// the Completed method have been called.
	/// </example>
	public class TransactionScope : IDisposable
	{
		#region Consts
		private const string TX_SCOPE_COUNT = "_TX_SCOPE_COUNT_";
		private bool _consistent = false;
		#endregion
		
		#region Fields
		private bool _closed = false;
		private TransactionScopeOptions _txScopeOptions;
		private TransactionOptions _txOptions;
		private static readonly ILog _logger = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );
		#endregion

		#region Properties


		/// <summary>
		/// Changes the vote to commit (true) or to abort (false).
		/// If all the TransactionScope instances involved in a 
		/// transaction have voted to commit, then the entire thing is committed.
		/// If any TransactionScope instances involved in a 
		/// transaction vote to abort, then the entire thing is aborted.
		/// </summary>
		private bool Consistent
		{
			set
			{
				_consistent = value;
			}
		} 

		/// <summary>
		/// Count of the TransactionScope that have been open.
		/// </summary>
		public int TransactionScopeCount 
		{
			get
			{
				object count = CallContext.GetData(TX_SCOPE_COUNT);
				return (count == null) ? 0 : (int)count;
			}
			set
			{
				CallContext.SetData(TX_SCOPE_COUNT, value);
			}
		}		
		
		/// <summary>
		/// Returns whether or not the current thread is in a transaction context.
		/// </summary>
		public static bool IsInTransaction
		{
			get
			{
				return ContextUtil.IsInTransaction;
			}
		}


		/// <summary>
		/// Gets the current value of the vote.
		/// </summary>
		public bool IsVoteCommit
		{
			get
			{
				return ContextUtil.MyTransactionVote == TransactionVote.Commit;
			}
		}
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// Creates a new instance with a TransactionScopeOptions.Required 
		/// and TransactionOptions.IsolationLevel.ReadCommitted.
		/// </summary>
		public TransactionScope()
		{
			_txOptions = new TransactionOptions();
			_txOptions.IsolationLevel = IsolationLevel.ReadCommitted;
			_txOptions.TimeOut  = new TimeSpan(0,0,0,15);

			_txScopeOptions = TransactionScopeOptions.Required;

			EnterTransactionContext();
		}

		 

		/// <summary>
		/// Creates a new instance with the specified TransactionScopeOptions
		///  and TransactionOptions.IsolationLevel.ReadCommitted.
		/// </summary>
		/// <param name="txScopeOptions">The specified TransactionScopeOptions</param>
		public TransactionScope(TransactionScopeOptions txScopeOptions)
		{
			_txOptions = new TransactionOptions();
			_txOptions.IsolationLevel = IsolationLevel.ReadCommitted;
			_txOptions.TimeOut  = new TimeSpan(0,0,0,15);

			_txScopeOptions = txScopeOptions;

			EnterTransactionContext();
		}


		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="txScopeOptions">The specified TransactionScopeOptions.</param>
		/// <param name="options">The specified TransactionOptions.</param>
		public TransactionScope(TransactionScopeOptions txScopeOptions, TransactionOptions options)
		{
			_txOptions = options;
			_txScopeOptions = txScopeOptions;

			EnterTransactionContext();
		}
		#endregion

		#region Methods

		/// <summary>
		/// 
		/// </summary>
		private void EnterTransactionContext()
		{
			if (++TransactionScopeCount == 1)
			{
				if (_logger.IsDebugEnabled)
				{
					_logger.Debug("Create a new ServiceConfig in ServiceDomain.");
				}

				ServiceConfig config = new ServiceConfig();

				config.TrackingEnabled = true;
				config.TrackingAppName = "iBATIS.NET";
				config.TrackingComponentName = "TransactionScope";
				config.TransactionDescription = "iBATIS.NET Distributed Transaction";
				config.Transaction = TransactionScopeOptions2TransactionOption( _txScopeOptions );
				config.TransactionTimeout = _txOptions.TimeOut.Seconds;
				config.IsolationLevel = IsolationLevel2TransactionIsolationLevel( _txOptions.IsolationLevel );

				// every call to ServiceDomain.Enter() creates a new COM+ Context 
				//(verified by calls to ContextUtil.ContextId), 
				// and every call to ServiceDomain.Leave() terminates that context
				ServiceDomain.Enter(config); 
			}

			_closed = false;

			if (_logger.IsDebugEnabled)
			{
				_logger.Debug("Open TransactionScope :"+ContextUtil.ContextId);
			}
		}



		/// <summary>
		/// Give the correpondance of a TransactionScopeOptions (à la Indigo) object in a TransactionOption (COM+) object
		/// </summary>
		/// <param name="transactionScopeOptions">The TransactionScopeOptions to macth.</param>
		/// <returns>The TransactionOption correspondance</returns>
		private TransactionOption TransactionScopeOptions2TransactionOption(TransactionScopeOptions transactionScopeOptions)
		{
			TransactionOption transactionOption;

			switch(transactionScopeOptions)
			{
				case TransactionScopeOptions.Mandatory:   
					throw new NotImplementedException("Will be used in Indigo.");
				case TransactionScopeOptions.NotSupported:   
					transactionOption = TransactionOption.NotSupported;
					break;
				case TransactionScopeOptions.Required:   
					transactionOption = TransactionOption.Required;
					break;
				case TransactionScopeOptions.RequiresNew:   
					transactionOption = TransactionOption.RequiresNew;
					break;
				case TransactionScopeOptions.Supported:   
					transactionOption = TransactionOption.Supported;
					break;
				default:            
					transactionOption = TransactionOption.Required;            
					break;
			}
			return transactionOption;
		}

		/// <summary>
		/// Give the correpondance of a TransactionIsolationLevel (à la Indigo) object in a IsolationLevel (COM+) object
		/// </summary>
		/// <param name="isolation">The IsolationLevel to macth.</param>
		/// <returns>The TransactionIsolationLevel correspondance</returns>
		private TransactionIsolationLevel IsolationLevel2TransactionIsolationLevel(IsolationLevel isolation)
		{
			TransactionIsolationLevel isolationLevel;

			switch(isolation)
			{
				case IsolationLevel.ReadCommitted:   
					isolationLevel = TransactionIsolationLevel.ReadCommitted;
					break;
				case IsolationLevel.ReadUncommitted:   
					isolationLevel = TransactionIsolationLevel.ReadUncommitted;
					break;
				case IsolationLevel.RepeatableRead:   
					isolationLevel = TransactionIsolationLevel.RepeatableRead;
					break;
				case IsolationLevel.Serializable:   
					isolationLevel = TransactionIsolationLevel.Serializable;
					break;
				case IsolationLevel.Unspecified:   
					throw new  NotImplementedException("Will be used in Indigo.");
				default:            
					isolationLevel = TransactionIsolationLevel.ReadCommitted;            
					break;
			}
			return isolationLevel;
		}

		/// <summary>
		/// Close the TransactionScope
		/// </summary>
		public void Close()
		{
			if (_closed == false)
			{
				if (_logger.IsDebugEnabled)
				{
					_logger.Debug("Close TransactionScope");
				}
				
				if (ContextUtil.IsInTransaction)
				{				
					if (_consistent==true && (this.IsVoteCommit))
					{
						ContextUtil.EnableCommit();
					}
					else
					{
						ContextUtil.DisableCommit();
					}
				}

				if (0 == --TransactionScopeCount)
				{
					if (_logger.IsDebugEnabled)
					{
						_logger.Debug("Leave in ServiceDomain ");
					}

					ServiceDomain.Leave();
				}

				_closed = true;
			}
		}

		/// <summary>
		/// Complete (commit) a transsaction
		/// </summary>
		public void Complete()
		{
			this.Consistent = true;
		}
		#endregion

		#region IDisposable Members
		/// <summary>
		/// Implementation of IDisposable so that this class 
		/// can be used with C#'s using statement.
		/// </summary>
		public void Dispose()
		{
			Close();
		}
		#endregion

	}
}
