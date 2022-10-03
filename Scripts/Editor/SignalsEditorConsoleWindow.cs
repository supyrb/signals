// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalsEditorConsoleWindow.cs">
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
using UnityEngine;

namespace Supyrb
{
	internal class SignalsEditorConsoleWindow : EditorWindow
	{
		[SerializeField]
		private TreeViewState treeViewState;
		
		[SerializeField]
		private List<SignalLogEntry> entries;

		private SignalLogEntry currentSelection;
		private SignalsLogTreeView treeView;
		private SearchField searchField;
		private SignalsTreeViewItem currentItem;
		
		private const float ToolbarHeight = 18f;
		private const float FooterHeight = 24f;

		private static class Styles
		{
			public static readonly GUIStyle FooterStyle;
			public static readonly GUIStyle ToolbarButton;
			public static readonly GUIContent Clear = EditorGUIUtility.TrTextContent("Clear", "Clear console entries");
			public static readonly GUIContent ClearOnPlay = EditorGUIUtility.TrTextContent("Clear on Play");
			public static readonly GUIContent ClearOnBuild = EditorGUIUtility.TrTextContent("Clear on Build");
			public static readonly GUIContent ClearOnRecompile = EditorGUIUtility.TrTextContent("Clear on Recompile");

			static Styles()
			{
				FooterStyle = new GUIStyle((GUIStyle) "ProjectBrowserBottomBarBg");
				ToolbarButton = new GUIStyle((GUIStyle) "ToolbarButton");
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
			
			if (entries == null)
			{
				entries = new List<SignalLogEntry>();
			}

			
			treeView = new SignalsLogTreeView(treeViewState, entries);
			treeView.OnSelectionChanged += OnSelectionChanged;
			searchField = new SearchField();
			searchField.downOrUpArrowKeyPressed += treeView.SetFocusAndEnsureSelectedItem;
			EditorSignalLog.Instance.Subscribe();
			EditorSignalLog.Instance.OnNewSignalLog += OnNewSignalLog;
			EditorSignalLog.Instance.OnClearLogs += Clear;
		}
		
		private void OnDisable()
		{
			if (searchField != null)
			{
				searchField.downOrUpArrowKeyPressed -= treeView.SetFocusAndEnsureSelectedItem;
			}

			EditorSignalLog.Instance.Unsubscribe();
			EditorSignalLog.Instance.OnNewSignalLog -= OnNewSignalLog;
			EditorSignalLog.Instance.OnClearLogs -= Clear;
		}

		private void OnInspectorUpdate()
		{
			this.Repaint();
		}

		private void OnGUI()
		{
			var contentHeight = this.position.height - ToolbarHeight - FooterHeight;

			DoToolbar();
			var rect = GUILayoutUtility.GetRect(0, this.position.width, 0, 100000);
			DoTreeView(rect);

			DoFooter();
		}
		
		private void DoToolbar()
		{
			GUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.Width(this.position.width),
				GUILayout.Height(ToolbarHeight));
			if (GUILayout.Button(Styles.Clear, Styles.ToolbarButton))
			{
				Clear();
				GUIUtility.keyboardControl = 0;
			}
			if (EditorGUILayout.DropdownButton(new GUIContent("Options"), FocusType.Passive, Styles.ToolbarButton))
			{
				var clearOnPlay = HasFlag(EditorSignalLog.ConsoleFlags.ClearOnPlay);
				var clearOnBuild = HasFlag(EditorSignalLog.ConsoleFlags.ClearOnBuild);
				var clearOnRecompile = HasFlag(EditorSignalLog.ConsoleFlags.ClearOnRecompile);

				GenericMenu menu = new GenericMenu();
				menu.AddItem(Styles.ClearOnPlay, clearOnPlay, () => { SetFlag(EditorSignalLog.ConsoleFlags.ClearOnPlay, !clearOnPlay); });
				menu.AddItem(Styles.ClearOnBuild, clearOnBuild, () => { SetFlag(EditorSignalLog.ConsoleFlags.ClearOnBuild, !clearOnBuild); });
				menu.AddItem(Styles.ClearOnRecompile, clearOnRecompile, () => { SetFlag(EditorSignalLog.ConsoleFlags.ClearOnRecompile, !clearOnRecompile); });
				var rect = GUILayoutUtility.GetLastRect();
				rect.y += EditorGUIUtility.singleLineHeight;
				menu.DropDown(rect);
			}
			
			
			GUILayout.Space(100);
			GUILayout.FlexibleSpace();
			treeView.searchString = searchField.OnToolbarGUI(treeView.searchString);

			GUILayout.EndHorizontal();
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
					lastDispatchedSignal.TimeStamp.DateTime,
					lastDispatchedSignal.SignalType.Name,
					lastDispatchedSignal.PlayDispatchTime);
				GUILayout.Label(signalText);
			}

			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		private void Clear()
		{
			treeView.ClearLogs();
		}

		private void OnNewSignalLog(SignalLogEntry logentry)
		{
			treeView.AddEntry(logentry);
		}
		
		private void OnSelectionChanged(SignalLogEntry selectedEntry)
		{
			currentSelection = selectedEntry;
		}

		private bool HasFlag(EditorSignalLog.ConsoleFlags flags)
		{
			return EditorSignalLog.Instance.HasFlag(flags);
		}

		private void SetFlag(EditorSignalLog.ConsoleFlags flags, bool val)
		{
			EditorSignalLog.Instance.SetFlag(flags, val);
		}
	}
}