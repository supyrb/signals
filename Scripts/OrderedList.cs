// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderedList.cs" company="Supyrb">
//   Copyright (c) 2019 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace Supyrb
{
	public class OrderedList<T>
	{
		private List<int> sortedOrders;
		private List<T> values;
		private bool uniqueValuesOnly;

		/// <summary>
		/// Ordered list with an order value that does not have to be unique and a value can be unique
		/// </summary>
		/// <param name="uniqueValuesOnly">If values should be unique</param>
		public OrderedList(bool uniqueValuesOnly)
		{
			this.uniqueValuesOnly = uniqueValuesOnly;
			this.sortedOrders = new List<int>();
			this.values = new List<T>();
		}

		public int Count => values.Count;

		public T this[int index]
		{
			get => values[index];
			set => values[index] = value;
		}

		public bool Add(int order, T value)
		{
			if (uniqueValuesOnly && Contains(value))
			{
				return false;
			}

			var index = GetSortedIndexFor(order);
			sortedOrders.Insert(index, order);
			values.Insert(index, value);
			return true;
		}

		public int IndexOf(T value)
		{
			return values.IndexOf(value);
		}

		public bool Remove(T value)
		{
			var index = IndexOf(value);
			if (index == -1)
			{
				return false;
			}

			sortedOrders.RemoveAt(index);
			values.RemoveAt(index);
			return true;
		}

		public void Clear()
		{
			sortedOrders.Clear();
			values.Clear();
		}

		public bool Contains(T item)
		{
			return IndexOf(item) != -1;
		}

		private int GetSortedIndexFor(int order)
		{
			var low = 0;
			var high = this.sortedOrders.Count;
			while (low < high)
			{
				var mid = (low + high) >> 1;
				if (this.sortedOrders[mid] < order)
				{
					low = mid + 1;
				}
				else
				{
					high = mid;
				}
			}

			return low;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return values.GetEnumerator();
		}
	}
}