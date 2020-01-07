// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalsTreeView.cs" company="Supyrb">
//   Copyright (c) 2020 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;

namespace Supyrb
{
	internal class SignalsTreeView : TreeView
	{
		public delegate void SelectionChangedDelegate(SerializableSystemType selectedType);

		public event SelectionChangedDelegate OnSelectionChanged;
		
		private List<TreeViewItem> signals;
		private List<SerializableSystemType> signalTypes;
		
		public SignalsTreeView(TreeViewState treeViewState)
			: base(treeViewState)
		{
			signals = new List<TreeViewItem>();
			signalTypes = SignalsEditorDatabase.Instance.SignalTypes;
			Reload();
		}

		protected override TreeViewItem BuildRoot()
		{
			signals.Clear();
			var root = new TreeViewItem {id = -1, depth = -1, displayName = "Root"};
			for (int i = 0; i < signalTypes.Count; i++)
			{
				var signalType = signalTypes[i];
				signals.Add(new TreeViewItem(i, 0, signalType.Name));
			}

			// Utility method that initializes the TreeViewItem.children and -parent for all items.
			SetupParentsAndChildrenFromDepths(root, signals);

			// Return root of the tree
			return root;
		}

		public void UpdateSignalData()
		{
			SignalsEditorDatabase.Instance.UpdateDatabase();
			this.Reload();
		}
		
		protected override void SelectionChanged(IList<int> selectedIds)
		{
			base.SelectionChanged(selectedIds);
			if (OnSelectionChanged == null)
			{
				return;
			}
			if (selectedIds.Count == 0)
			{
				OnSelectionChanged(null);
			}
			else
			{
				OnSelectionChanged(signalTypes[selectedIds[0]]);
			}
		}
	}
}