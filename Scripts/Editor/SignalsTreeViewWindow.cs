using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEditor.IMGUI.Controls;


namespace Supyrb
{
	class SignalsTreeViewWindow : EditorWindow
	{
		[SerializeField] 
		private TreeViewState treeViewState;
		
		private SignalsTreeView treeView;
		private SearchField searchField;

		private const float hierarchyWidth = 250f;

		private static class Styles
		{
			public static GUIStyle HierarchyStyle = (GUIStyle) "OL box";
		}

		void OnEnable ()
		{
			// Check if we already had a serialized view state (state 
			// that survived assembly reloading)
			if (treeViewState == null)
				treeViewState = new TreeViewState ();

			treeView = new SignalsTreeView(treeViewState);
			searchField = new SearchField ();
			searchField.downOrUpArrowKeyPressed += treeView.SetFocusAndEnsureSelectedItem;
		}

		void OnGUI ()
		{
			var contentHeight = this.position.height - EditorGUIUtility.singleLineHeight;
			
			DoToolbar ();
			EditorGUILayout.BeginHorizontal();

			
			EditorGUILayout.BeginVertical(Styles.HierarchyStyle, GUILayout.Width(hierarchyWidth));
			Rect rect = GUILayoutUtility.GetRect(0, hierarchyWidth, 0, 100000);
			DoTreeView(rect);
			EditorGUILayout.EndVertical();
			
			EditorGUILayout.Separator();
			
			GUILayout.Label("Content");

			EditorGUILayout.EndHorizontal();
			
		}

		void DoToolbar()
		{
			GUILayout.BeginHorizontal (EditorStyles.toolbar);
			GUILayout.Space (100);
			GUILayout.FlexibleSpace();
			treeView.searchString = searchField.OnToolbarGUI (treeView.searchString);
			GUILayout.EndHorizontal();
		}

		void DoTreeView(Rect rect)
		{
			treeView.OnGUI(rect);
		}

		// Add menu named "My Window" to the Window menu
		[MenuItem ("Supyrb/Simple Tree Window")]
		static void ShowWindow ()
		{
			// Get existing open window or if none, make a new one:
			var window = GetWindow<SignalsTreeViewWindow> ();
			window.titleContent = new GUIContent ("Signals");
			window.Show ();
		}
	}
}
