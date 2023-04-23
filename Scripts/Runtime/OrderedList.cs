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
		private readonly int defaultOrderValue;
		private int defaultOrderNextIndex;

		public OrderedList(bool uniqueValuesOnly) : this(uniqueValuesOnly, 0)
		{}

		/// <summary>
		/// Ordered list with an order value that does not have to be unique and a value can be unique
		/// </summary>
		/// <param name="uniqueValuesOnly">If values should be unique</param>
		/// <param name="defaultOrder">default sorting value - does not need search when adding value</param>
		public OrderedList(bool uniqueValuesOnly, int defaultOder)
		{
			this.uniqueValuesOnly = uniqueValuesOnly;
			this.sortedOrders = new List<int>();
			this.values = new List<T>();
			this.defaultOrderValue = defaultOder;
			this.defaultOrderNextIndex = 0;
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
		/// Add an item to the ordered list with order <see cref="defaultOrderValue"/>
		/// </summary>
		/// <param name="value">Value to be added</param>
		/// <returns>The index at which the value was added, or -1 if the value was already in the list and only unique values are allowed</returns>
		public int Add(T value)
		{
			return Add(defaultOrderValue, value);
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

			var index = order == defaultOrderValue ? defaultOrderNextIndex : GetSortedIndexFor(order);
			sortedOrders.Insert(index, order);
			values.Insert(index, value);
			if(order <= defaultOrderNextIndex)
			{
				defaultOrderNextIndex++;
			}
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
			if(index < defaultOrderNextIndex)
			{
				defaultOrderNextIndex--;
			}
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
			defaultOrderNextIndex = 0;
		}

		public bool Contains(T item)
		{
			return IndexOf(item) != -1;
		}

		/// <summary>
		/// Returns the index after the last value of order
		/// This way new entries with the same order value will be added at the end of the entries
		/// </summary>
		/// <param name="order">Order value to search for</param>
		/// <returns></returns>
		private int GetSortedIndexFor(int order)
		{
			var low = 0;
			var high = this.sortedOrders.Count;
			while (low < high)
			{
				var mid = (low + high) >> 1;
				if (this.sortedOrders[mid] > order)
				{
					high = mid;
				}
				else
				{
					low = mid + 1;
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