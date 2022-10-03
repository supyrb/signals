// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalsAboutWindow.cs" company="Supyrb">
//   Copyright (c) 2020 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Supyrb
{
	public class SignalsAboutWindow : EditorWindow
	{
		[Serializable]
		private class UnityPackage
		{
			public string version;
		}
		
		private const string Headline = "Signals ({0})";
		private static string version;
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

		[MenuItem("Window/Signals/About")]
		public static void ShowWindow()
		{
			var window = ScriptableObject.CreateInstance<SignalsAboutWindow>();
			version = window.GetPackageVersion();
			window.titleContent = new GUIContent(string.Format(Headline, version));
			window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
			window.ShowModalUtility();
		}

		private string GetPackageVersion()
		{
			MonoScript monoScript = MonoScript.FromScriptableObject(this);
			var assetPath = AssetDatabase.GetAssetPath(monoScript);
			DirectoryInfo projectDirectory = new DirectoryInfo(Application.dataPath).Parent;
			DirectoryInfo directory = new FileInfo(assetPath).Directory;
			while (directory.GetFiles("package.json").Length == 0 && directory != projectDirectory)
			{
				directory = directory.Parent;
			}

			if (directory == projectDirectory)
			{
				return null;
			}

			FileInfo fileInfo = directory.GetFiles("package.json")[0];
			string packageContent = File.ReadAllText(fileInfo.FullName);
			UnityPackage package = JsonUtility.FromJson<UnityPackage>(packageContent);
			return package.version;
		}

		private void OnGUI()
		{
			GUILayout.Space(12);
			EditorGUILayout.LabelField(string.Format(Headline, version), Styles.HeaderLabel);
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