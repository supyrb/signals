// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalsTreeViewWindow.cs" company="Supyrb">
//   Copyright (c) 2020 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Supyrb
{
	internal class SignalsTreeViewWindow : EditorWindow
	{
		[SerializeField]
		private TreeViewState treeViewState;

		private SignalsTreeView treeView;
		private SearchField searchField;
		private SignalsTreeViewItems items;
		private SerializableSystemType currentSelection;
		private SignalsTreeViewItem currentItem;

		private const float HierarchyWidth = 250f;
		private const float ToolbarHeight = 18f;
		private const float FooterHeight = 26f;

		private static class Styles
		{
			public static GUIStyle HierarchyStyle = (GUIStyle) "OL box";
			public static GUIStyle FooterStyle = (GUIStyle) "ProjectBrowserBottomBarBg";
		}

		private void OnEnable()
		{
			// Check if we already had a serialized view state (state 
			// that survived assembly reloading)
			if (treeViewState == null)
			{
				treeViewState = new TreeViewState();
			}

			items = new SignalsTreeViewItems();
			treeView = new SignalsTreeView(treeViewState);
			treeView.OnSelectionChanged += OnSelectionChanged;
			searchField = new SearchField();
			searchField.downOrUpArrowKeyPressed += treeView.SetFocusAndEnsureSelectedItem;
			
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
		}

		private void OnDisable()
		{
			if (searchField != null)
			{
				searchField.downOrUpArrowKeyPressed -= treeView.SetFocusAndEnsureSelectedItem;
			}
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
		}
		
		void OnInspectorUpdate()
		{
			this.Repaint();
		}
		
		private void OnPlayModeStateChanged(PlayModeStateChange state)
		{
			if (state == PlayModeStateChange.EnteredEditMode)
			{
				items.ResetInstances();
			}
		}

		private void OnGUI()
		{
			var contentHeight = this.position.height - ToolbarHeight - FooterHeight;

			DoToolbar();
			EditorGUILayout.BeginHorizontal(GUILayout.Height(contentHeight));


			EditorGUILayout.BeginVertical(Styles.HierarchyStyle, GUILayout.Width(HierarchyWidth));
			var rect = GUILayoutUtility.GetRect(0, HierarchyWidth, 0, 100000);
			DoTreeView(rect);
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical();
			DoDetailView();
			EditorGUILayout.EndVertical();

			EditorGUILayout.EndHorizontal();

			DoFooter();
		}

		private void DoToolbar()
		{
			GUILayout.BeginHorizontal(EditorStyles.toolbar,
				GUILayout.Width(this.position.width),
				GUILayout.Height(ToolbarHeight));
			GUILayout.Space(100);
			GUILayout.FlexibleSpace();
			treeView.searchString = searchField.OnToolbarGUI(treeView.searchString);
			GUILayout.EndHorizontal();
		}

		private void DoTreeView(Rect rect)
		{
			treeView.OnGUI(rect);
		}

		private void DoDetailView()
		{
			if (currentSelection == null)
			{
				GUILayout.Label("Nothing selected");
				return;
			}

			if (currentItem == null)
			{
				currentItem = items.Get(currentSelection.SystemType);
			}

			if (currentItem == null)
			{
				GUILayout.Label("Only signals derived from ASignal supported");
				return;
			}

			currentItem.DrawSignalDetailView();
		}

		private void DoFooter()
		{
			GUILayout.BeginHorizontal(Styles.FooterStyle,
				GUILayout.Width(this.position.width),
				GUILayout.Height(FooterHeight));
			GUILayout.Space(100);
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Update Signal List"))
			{
				treeView.UpdateSignalData();
				items.Clear();
			}

			GUILayout.EndHorizontal();
		}

		
		[MenuItem("Window/Signals")]
		private static void ShowWindow()
		{
			// Get existing open window or if none, make a new one:
			var window = GetWindow<SignalsTreeViewWindow>();
			window.titleContent = new GUIContent("Signals");
			window.Show();
		}

		private void OnSelectionChanged(SerializableSystemType selectedtype)
		{
			currentSelection = selectedtype;
			currentItem = items.Get(currentSelection.SystemType);
		}
	}
}