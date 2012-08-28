
using System;
using System.Collections;


namespace IBatisNet.DataMapper.Test.Domain 
{
	public class AccountCollection : CollectionBase 
	{
		public AccountCollection() {}

		public Account this[int index] 
		{
			get	{ return (Account)List[index]; }
			set { List[index] = value; }
		}

		public int Add(Account value) 
		{
			return List.Add(value);
		}

		public void AddRange(Account[] value) 
		{
			for (int i = 0;	i < value.Length; i++) 
			{
				Add(value[i]);
			}
		}

		public void AddRange(AccountCollection value) 
		{
			for (int i = 0;	i < value.Count; i++) 
			{
				Add(value[i]);
			}
		}

		public bool Contains(Account value) 
		{
			return List.Contains(value);
		}

		public void CopyTo(Account[] array, int index) 
		{
			List.CopyTo(array, index);
		}

		public int IndexOf(Account value) 
		{
			return List.IndexOf(value);
		}
		
		public void Insert(int index, Account value) 
		{
			List.Insert(index, value);
		}
		
		public void Remove(Account value) 
		{
			List.Remove(value);
		}
	}
}
