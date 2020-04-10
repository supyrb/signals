// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalsAboutWindow.cs" company="Supyrb">
//   Copyright (c) 2020 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace Supyrb
{
	public class SignalsAboutWindow : EditorWindow
	{
		private const string Headline = "Signals (0.5.0)";
		private static class Styles
		{
			internal static GUIStyle HeaderLabel;
			internal static GUIStyle LinkLabel;

			static Styles()
			{
				HeaderLabel = new GUIStyle((GUIStyle) "AM MixerHeader");
				HeaderLabel.alignment = TextAnchor.MiddleCenter;
				HeaderLabel.fixedHeight = 32;

				LinkLabel = new GUIStyle(EditorStyles.linkLabel);
				LinkLabel.alignment = TextAnchor.MiddleCenter;
			}
		}

		
		public static void Init()
		{
			var window = ScriptableObject.CreateInstance<SignalsAboutWindow>();
			window.titleContent = new GUIContent(Headline);
			window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
			window.ShowModalUtility();
		}

		private void OnGUI()
		{
			GUILayout.Space(12);
			EditorGUILayout.LabelField(Headline, Styles.HeaderLabel);
			GUILayout.Space(12);
			DrawLink("Github", "https://www.github.com/supyrb/signals");
			DrawLink("OpenUPM", "https://openupm.com/packages/com.supyrb.signals/");
			DrawLink("Create Issue", "https://github.com/supyrb/signals/issues/new");
			DrawLink("Contact us", "mailto:pr@supyrb.com");

			GUILayout.FlexibleSpace();
		}

		private static void DrawLink(string text, string url)
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button(text, Styles.LinkLabel))
			{
				Application.OpenURL(url);
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
		}
	}
}