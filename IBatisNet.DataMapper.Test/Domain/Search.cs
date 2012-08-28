using System;

namespace IBatisNet.DataMapper.Test.Domain
{
	/// <summary>
	/// Summary description for Search.
	/// </summary>
	public class Search
	{
		private int _numberSearch;
		private DateTime _startDate;
		private string _operande;
		private bool _startDateAnd;
		
		public bool StartDateAnd
		{
			get
			{
				return _startDateAnd; 
			}
			set
			{ 
				_startDateAnd = value; 
			}
		}

		public string Operande
		{
			get
			{
				return _operande; 
			}
			set
			{ 
				_operande = value; 
			}
		}

		public DateTime StartDate
		{
			get
			{
				return _startDate; 
			}
			set
			{ 
				_startDate = value; 
			}
		}

		public int NumberSearch
		{
			get
			{
				return _numberSearch; 
			}
			set
			{ 
				_numberSearch = value; 
			}
		}
	}
}
