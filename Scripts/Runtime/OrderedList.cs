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
		private readonly List<int> sortedOrders;
		private readonly List<T> values;
		private readonly bool uniqueValuesOnly;

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

		public int Count
		{
			get { return values.Count; }
		}

		public T this[int index]
		{
			get { return values[index]; }
			set { values[index] = value; }
		}

		public int GetSortOrderForIndex(int index)
		{
			return sortedOrders[index];
		}

		/// <summary>
		/// Add an item to the ordered list
		/// </summary>
		/// <param name="order">Value after which the list is sorted (Ascending)</param>
		/// <param name="value">Value to be added</param>
		/// <returns>The index at which the value was added, or -1 if the value was already in the list and only unique values are allowed</returns>
		public int Add(int order, T value)
		{
			if (uniqueValuesOnly && Contains(value))
			{
				return -1;
			}

			var index = GetSortedIndexFor(order);
			sortedOrders.Insert(index, order);
			values.Insert(index, value);
			return index;
		}

		/// <summary>
		/// Remove an item from the ordered list
		/// </summary>
		/// <param name="value">Item to remove</param>
		/// <returns>The index at which the item was removed, or -1 if the list didn't contain the item</returns>
		public int Remove(T value)
		{
			var index = IndexOf(value);
			if (index == -1)
			{
				return -1;
			}

			sortedOrders.RemoveAt(index);
			values.RemoveAt(index);
			return index;
		}
		
		public int IndexOf(T value)
		{
			return values.IndexOf(value);
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