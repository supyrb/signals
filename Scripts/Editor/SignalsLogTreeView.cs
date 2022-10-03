// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalsLogTreeView.cs">
//   Copyright (c) 2022 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace Supyrb
{
	internal class SignalsLogTreeView : TreeView
	{
		public delegate void SelectionChangedDelegate(SignalLogEntry selectedEntry);

		public event SelectionChangedDelegate OnSelectionChanged;
		
		private List<TreeViewItem> treeItems;
		private List<SignalLogEntry> entries;
		private TreeViewItem root;
		
		public SignalsLogTreeView(TreeViewState treeViewState, List<SignalLogEntry> entries)
			: base(treeViewState)
		{
			rowHeight = EditorGUIUtility.singleLineHeight * 2f;
			showAlternatingRowBackgrounds = true;
			this.entries = entries;
			treeItems = new List<TreeViewItem>();
			root = new TreeViewItem {id = -1, depth = -1, displayName = "Root"};
			Reload();
		}

		public void ClearLogs()
		{
			entries.Clear();
			treeItems.Clear();
			Reload();
		}

		public void AddEntry(SignalLogEntry entry)
		{
			entries.Add(entry);
			Reload();
		}

		protected override TreeViewItem BuildRoot()
		{
			for (int i = treeItems.Count; i < entries.Count; i++)
			{
				var entry = entries[i];
				var signalText = $"[{entry.TimeStamp.DateTime:HH:mm:ss}] {entry.SignalType.Name} - " +
								$"Dispatch Time: {entry.PlayDispatchTime:0.000}\n" +
								$"{entry.SourceFileName}:{entry.MemberName} (at {entry.SourceFilePath}:{entry.SourceLineNumber}";
				treeItems.Add(new TreeViewItem(i, 0, signalText));
			}

			// Utility method that initializes the TreeViewItem.children and -parent for all items.
			SetupParentsAndChildrenFromDepths(root, treeItems);

			// Return root of the tree
			return root;
		}

		protected override void SelectionChanged(IList<int> selectedIds)
		{
			base.SelectionChanged(selectedIds);
			if (OnSelectionChanged == null)
			{
				return;
			}
			if (selectedIds.Count == 0 || entries.Count == 0)
			{
				OnSelectionChanged(null);
			}
			else
			{
				OnSelectionChanged(entries[selectedIds[0]]);
			}
		}
	}
}