// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalsEditorWindow.cs" company="Supyrb">
//   Copyright (c) 2020 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Supyrb
{
	/// <summary>
	/// General signals editor window showing all available signals in the project
	/// Can be used to debug and dispatch signals in the editor
	/// </summary>
	internal class SignalsEditorWindow : EditorWindow
	{
		[SerializeField]
		private TreeViewState treeViewState;

		private SignalsTreeView treeView;
		private SearchField searchField;
		private SignalsTreeViewItems items;
		private SerializableSystemType currentSelection;
		private SignalsTreeViewItem currentItem;
		private GUIContent refreshSignalListGuiContent;
		private GUIContent aboutSignalsGuiContent;
		private Vector2 detailScrollPosition = Vector2.zero;

		private const float HierarchyWidth = 250f;
		private const float ToolbarHeight = 18f;
		private const float FooterHeight = 24f;

		private static class Styles
		{
			public static GUIStyle HierarchyStyle;
			public static GUIStyle FooterStyle;
			public static GUIStyle IconButton;

			static Styles()
			{
				HierarchyStyle = new GUIStyle((GUIStyle) "OL box");
				FooterStyle = new GUIStyle((GUIStyle) "ProjectBrowserBottomBarBg");
				IconButton = new GUIStyle((GUIStyle) "IconButton");
				IconButton.margin.top = 3;
			}
		}
		
		[MenuItem("Window/Signals/Signals")]
		private static void ShowWindow()
		{
			// Get existing open window or if none, make a new one:
			var window = GetWindow<SignalsEditorWindow>();
			var titleContent = EditorGUIUtility.IconContent("Profiler.NetworkMessages");
			titleContent.text = "Signals";
			window.titleContent = titleContent;
			window.Show();
		}

		private void OnEnable()
		{
			// Check if we already had a serialized view state
			// (state that survived assembly reloading)
			if (treeViewState == null)
			{
				treeViewState = new TreeViewState();
			}

			items = new SignalsTreeViewItems();
			treeView = new SignalsTreeView(treeViewState);
			treeView.OnSelectionChanged += OnSelectionChanged;
			searchField = new SearchField();
			searchField.downOrUpArrowKeyPressed += treeView.SetFocusAndEnsureSelectedItem;

			refreshSignalListGuiContent = EditorGUIUtility.IconContent("Refresh");
			refreshSignalListGuiContent.tooltip = "Refresh Signal list";

			aboutSignalsGuiContent = EditorGUIUtility.IconContent("_Help");
			aboutSignalsGuiContent.tooltip = "About";
			
			EditorSignalLog.Instance.Subscribe();
			EditorSignalLog.Instance.OnClearLogs += Clear;
		}

		private void OnDisable()
		{
			if (searchField != null)
			{
				searchField.downOrUpArrowKeyPressed -= treeView.SetFocusAndEnsureSelectedItem;
			}

			EditorSignalLog.Instance.Unsubscribe();
			EditorSignalLog.Instance.OnClearLogs -= Clear;
		}

		private void OnInspectorUpdate()
		{
			this.Repaint();
		}

		private void Clear()
		{
			items.Reset();
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

			var detailWidth = this.position.width - HierarchyWidth;
			detailScrollPosition = GUILayout.BeginScrollView(detailScrollPosition, GUILayout.Width(detailWidth), GUILayout.Height(contentHeight));
			{
				DoDetailView();
				GUILayout.Space(EditorGUIUtility.standardVerticalSpacing * 4f);
			}
			GUILayout.EndScrollView();

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
			if (GUILayout.Button(aboutSignalsGuiContent, Styles.IconButton))
			{
				SignalsAboutWindow.ShowWindow();
			}

			GUILayout.EndHorizontal();
		}

		private void DoTreeView(Rect rect)
		{
			treeView.OnGUI(rect);
		}

		private void DoDetailView()
		{
			if (currentSelection == null || currentSelection.SystemType == null)
			{
				GUILayout.Label("Nothing selected");
				return;
			}

			if (currentItem == null)
			{
				currentItem = items.Get(currentSelection.SystemType);
			}

			currentItem.DrawSignalDetailView();
		}

		private void DoFooter()
		{
			GUILayout.BeginHorizontal(Styles.FooterStyle,
				GUILayout.Width(this.position.width),
				GUILayout.Height(FooterHeight));

			var lastDispatchedSignal = SignalLog.Instance.GetLastEntry();
			if (lastDispatchedSignal != null)
			{
				var signalText = string.Format("[{0:HH:mm:ss}] {1} - Dispatch Time: {2:0.000}",
					lastDispatchedSignal.TimeStamp.DateTime,
					lastDispatchedSignal.SignalType.Name,
					lastDispatchedSignal.PlayDispatchTime);
				GUILayout.Label(signalText);
			}

			GUILayout.FlexibleSpace();
			if (GUILayout.Button(refreshSignalListGuiContent, Styles.IconButton))
			{
				treeView.UpdateSignalData();
				items.Clear();
			}

			GUILayout.EndHorizontal();
		}

		private void OnSelectionChanged(SerializableSystemType selectedtype)
		{
			currentSelection = selectedtype;
			currentItem = items.Get(currentSelection.SystemType);
			detailScrollPosition.x = 0f;
			detailScrollPosition.y = 0f;
		}
	}
}