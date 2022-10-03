// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalLogViewDrawer.cs" company="Supyrb">
//   Copyright (c) 2020 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Supyrb
{
	internal class SignalLogViewDrawer
	{
		private static class Styles
		{
			internal static GUIStyle EvenEntry;
			internal static GUIStyle OddEntry;

			static Styles()
			{
				EvenEntry = new GUIStyle((GUIStyle) "CN EntryBackEven");
				OddEntry = new GUIStyle((GUIStyle) "CN EntryBackOdd");

				EvenEntry.padding.left = 0;
				OddEntry.padding.left = 0;
				
			}
		}

		private Type type;
		private List<SignalLogEntry> signalLog;
		private Vector2 scrollPos;
		private const float maxHeight = 200f;
		private const int maxEntries = 100;

		public SignalLogViewDrawer(Type type)
		{
			this.type = type;
			signalLog = new List<SignalLogEntry>();
			scrollPos = new Vector2(0f, 0f);
		}

		public void Reset()
		{
			signalLog.Clear();
			scrollPos.x = 0f;
			scrollPos.y = 0f;
		}

		public void Update()
		{
			var newEntries = SignalLog.Instance.GetLogEntriesForType(type, ref signalLog);
			if (newEntries)
			{
				scrollPos.y = Mathf.Max(0f, GetListContentHeight() - maxHeight);
			}
		}

		public void DrawLogsForCurrentType()
		{
			if (signalLog.Count == 0)
			{
				GUILayout.Label("No logs captured for this signal");
				return;
			}

			var height = Mathf.Min(GetListContentHeight(), maxHeight);

			scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Height(height));
			{
				var startIndex = 0;
				if (signalLog.Count > maxEntries)
				{
					startIndex = signalLog.Count - maxEntries;
					var style = GetStyleForIndex(startIndex - 1);
					var text = string.Format("Hiding {0} older entries", startIndex);
					
					GUILayout.Label(text, style);
				}
				
				for (var i = startIndex; i < signalLog.Count; i++)
				{
					var entry = signalLog[i];
					var style = GetStyleForIndex(i);

					var text = $"[{entry.TimeStamp.DateTime:HH:mm:ss}] {i:000} " +
								$"from {entry.SourceFileName}:{entry.MemberName}:(Line {entry.SourceLineNumber})";
					
					GUILayout.Label(text, style);
				}
			}
			GUILayout.EndScrollView();
		}

		private static GUIStyle GetStyleForIndex(int index)
		{
			return index % 2 == 0 ? Styles.EvenEntry : Styles.OddEntry;
		}

		/// <summary>
		/// Get the editor height of the complete list with all entries
		/// </summary>
		/// <returns>Total list content height</returns>
		private float GetListContentHeight()
		{
			var entries = signalLog.Count;
			if (entries > maxEntries)
			{
				entries = maxEntries;
			}
			
			// TODO get values from elements instead of hardcoding them
			return 8f + entries * 28f;
		}
	}
}