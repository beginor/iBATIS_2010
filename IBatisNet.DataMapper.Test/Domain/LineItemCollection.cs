
using System;
using System.Collections;

namespace IBatisNet.DataMapper.Test.Domain 
{
	/// <summary>
	/// In order to make this class strongly-proxified you have to mark virtual 
	/// each property/method you want be proxified.
	/// </summary>
	[Serializable]
	public class LineItemCollection : CollectionBase 
	{
		public LineItemCollection() {}

		public virtual LineItem this[int index] 
		{
			get	{ return (LineItem)List[index]; }
			set { List[index] = value; }
		}

		public virtual int Add(LineItem value) 
		{
			return List.Add(value);
		}

		public virtual void AddRange(LineItem[] value) 
		{
			for (int i = 0;	i < value.Length; i++) 
			{
				Add(value[i]);
			}
		}

		public virtual void AddRange(LineItemCollection value) 
		{
			for (int i = 0;	i < value.Count; i++) 
			{
				Add(value[i]);
			}
		}

		public virtual bool Contains(LineItem value) 
		{
			return List.Contains(value);
		}

		public virtual void CopyTo(LineItem[] array, int index) 
		{
			List.CopyTo(array, index);
		}

		public virtual int IndexOf(LineItem value) 
		{
			return List.IndexOf(value);
		}
		
		public virtual void Insert(int index, LineItem value) 
		{
			List.Insert(index, value);
		}
		
		public virtual void Remove(LineItem value) 
		{
			List.Remove(value);
		}

		public new virtual int Count
		{
			get {return this.List.Count;}
		}
	}
}
