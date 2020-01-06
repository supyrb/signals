// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalsTreeView.cs" company="Supyrb">
//   Copyright (c) 2020 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;

namespace Supyrb
{
	internal class SignalsTreeView : TreeView
	{
		private List<TreeViewItem> signals;
		public SignalsTreeView(TreeViewState treeViewState)
			: base(treeViewState)
		{
			signals = new List<TreeViewItem>();
			Reload();
		}

		protected override TreeViewItem BuildRoot()
		{
			var signalTypes = SignalsEditorDatabase.Instance.SignalTypes;
			signals.Clear();
			var root = new TreeViewItem {id = 0, depth = -1, displayName = "Root"};
			for (int i = 0; i < signalTypes.Count; i++)
			{
				var signalType = signalTypes[i];
				signals.Add(new TreeViewItem(i+1, 0, signalType.Name));
			}

			// Utility method that initializes the TreeViewItem.children and -parent for all items.
			SetupParentsAndChildrenFromDepths(root, signals);

			// Return root of the tree
			return root;
		}
	}
}