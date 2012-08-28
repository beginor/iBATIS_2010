using System;
using System.Collections;

namespace IBatisNet.DataMapper.Test.Domain 
{
	public class DocumentCollection : CollectionBase 
	{
		public DocumentCollection() {}

		public Document this[int index] {
			get	{ return (Document)List[index]; }
			set { List[index] = value; }
		}

		public int Add(Document value) {
			return List.Add(value);
		}

		public void AddRange(Document[] value) {
			for (int i = 0;	i < value.Length; i++) {
				Add(value[i]);
			}
		}

		public void AddRange(DocumentCollection value) {
			for (int i = 0;	i < value.Count; i++) {
				Add(value[i]);
			}
		}

		public bool Contains(Document value) {
			return List.Contains(value);
		}

		public void CopyTo(Document[] array, int index) {
			List.CopyTo(array, index);
		}

		public int IndexOf(Document value) {
			return List.IndexOf(value);
		}
		
		public void Insert(int index, Document value) {
			List.Insert(index, value);
		}
		
		public void Remove(Document value) {
			List.Remove(value);
		}
	}
}
