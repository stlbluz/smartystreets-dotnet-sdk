﻿namespace SmartyStreets
{
	using System.Collections;
	using System.Collections.Generic;

	public class Batch<T> : ICollection<T>
		where T : class, ILookup
	{
		private readonly int maxSize;
		private readonly Dictionary<string, T> named;
		private readonly List<T> all;

		public Batch(int maxSize)
		{
			this.maxSize = maxSize;
			this.named = new Dictionary<string, T>();
			this.all = new List<T>();
		}

		internal byte[] Serialize(ISerializer serializer)
		{
			return serializer.Serialize(this.all);
		}

		public void Add(T newAddress)
		{
			if (this.all.Count >= this.maxSize)
				throw new BatchFullException("Batch size cannot exceed " + this.maxSize);

			this.all.Add(newAddress);

			var key = newAddress.InputId;
			if (key == null)
				return;

			this.named[key] = newAddress;
		}
		public bool Contains(T item)
		{
			return this.all.Contains(item);
		}
		public bool Remove(T item)
		{
			this.named.Remove(item.InputId);
			return this.all.Remove(item);
		}
		public void Clear()
		{
			this.named.Clear();
			this.all.Clear();
		}
		public void CopyTo(T[] array, int arrayIndex)
		{
			this.all.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return this.all.Count; }
		}
		public bool IsReadOnly
		{
			get { return false; }
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this.all.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public T this[string value]
		{
			get { return this.named[value]; }
		}
		public T this[int index]
		{
			get { return this.all[index]; }
		}
	}
}