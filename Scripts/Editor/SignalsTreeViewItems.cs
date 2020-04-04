// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalsTreeViewItems.cs" company="Supyrb">
//   Copyright (c) 2020 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Supyrb
{
	public class SignalsTreeViewItems
	{
		private Dictionary<Type, SignalsTreeViewItem> items;

		public SignalsTreeViewItems()
		{
			items = new Dictionary<Type, SignalsTreeViewItem>();
		}

		public SignalsTreeViewItem Get(Type type)
		{
			SignalsTreeViewItem item;
			if (items.TryGetValue(type, out item))
			{
				return item;
			}

			item = new SignalsTreeViewItem(type);
			items[type] = item;
			return item;
		}

		public void Clear()
		{
			items.Clear();
		}

		public void Reset()
		{
			foreach (var item in items)
			{
				item.Value.Reset();
			}
		}
	}
}