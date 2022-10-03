// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalsEditorConsoleWindow.cs">
//   Copyright (c) 2022 Johannes Deml. All rights reserved.
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
	internal class SignalsEditorConsoleWindow : EditorWindow
	{
		[SerializeField]
		private TreeViewState treeViewState;
		
		private SignalLogEntry currentSelection;
		private SignalsLogTreeView treeView;
		private SearchField searchField;
		private SignalsTreeViewItem currentItem;
		
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
		
		[MenuItem("Window/Signals/Console")]
		private static void ShowWindow()
		{
			// Get existing open window or if none, make a new one:
			var window = GetWindow<SignalsEditorConsoleWindow>();
			var titleContent = EditorGUIUtility.IconContent("Profiler.NetworkMessages");
			titleContent.text = "Signals Console";
			window.titleContent = titleContent;
			window.Show();
		}

		private void OnEnable()
		{
			// Check if we already had a serialized view state
			//  (state that survived assembly reloading)
			if (treeViewState == null)
			{
				treeViewState = new TreeViewState();
			}

			treeView = new SignalsLogTreeView(treeViewState);
			treeView.OnSelectionChanged += OnSelectionChanged;
			searchField = new SearchField();
			searchField.downOrUpArrowKeyPressed += treeView.SetFocusAndEnsureSelectedItem;
			SignalLog.Instance.Subscribe();
			SignalLog.Instance.OnNewSignalLog += OnNewSignalLog;

			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
		}
		
		private void OnDisable()
		{
			if (searchField != null)
			{
				searchField.downOrUpArrowKeyPressed -= treeView.SetFocusAndEnsureSelectedItem;
			}

			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
			SignalLog.Instance.Unsubscribe();
			SignalLog.Instance.OnNewSignalLog -= OnNewSignalLog;
		}

		private void OnInspectorUpdate()
		{
			this.Repaint();
		}

		private void OnPlayModeStateChanged(PlayModeStateChange state)
		{
			if (state == PlayModeStateChange.ExitingEditMode)
			{
				treeView.ClearLogs();
				SignalLog.Instance.Clear();
				// TODO do we really need to do this?
				SignalLog.Instance.Unsubscribe();
				SignalLog.Instance.Subscribe();
			}
		}

		private void OnGUI()
		{
			var contentHeight = this.position.height - FooterHeight;

			EditorGUILayout.BeginHorizontal(GUILayout.Height(contentHeight));


			var rect = GUILayoutUtility.GetRect(0, this.position.width, 0, 100000);
			DoTreeView(rect);

			EditorGUILayout.EndHorizontal();

			DoFooter();
		}

		private void DoTreeView(Rect rect)
		{
			treeView.OnGUI(rect);
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
					lastDispatchedSignal.TimeStamp,
					lastDispatchedSignal.SignalType.Name,
					lastDispatchedSignal.PlayDispatchTime);
				GUILayout.Label(signalText);
			}

			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		private void OnNewSignalLog(SignalLogEntry logentry)
		{
			treeView.AddEntry(logentry);
		}
		
		private void OnSelectionChanged(SignalLogEntry selectedEntry)
		{
			currentSelection = selectedEntry;
		}
	}
}