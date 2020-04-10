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
		private List<SignalLogItem> signalLog;
		private Vector2 scrollPos;
		private const float maxHeight = 200f;
		private const int maxEntries = 100;

		public SignalLogViewDrawer(Type type)
		{
			this.type = type;
			signalLog = new List<SignalLogItem>();
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
			var newEntries = SignalLog.Instance.UpdateLog(type, ref signalLog);
			if (newEntries)
			{
				scrollPos.y = Mathf.Max(0f, GetListContentHeight() - maxHeight);
			}
		}

		public void DrawLog()
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
					var style = GetStyleForEntry(startIndex - 1);
					var text = string.Format("Hiding {0} older entries", startIndex);
					
					GUILayout.Label(text, style);
				}
				
				for (var i = startIndex; i < signalLog.Count; i++)
				{
					var entry = signalLog[i];
					var style = GetStyleForEntry(i);

					var text = string.Format("{0:000} - [{1:HH:mm:ss}] - Time.time: {2:0.00}", 
						i, entry.TimeStamp, entry.PlayDispatchTime);
					
					GUILayout.Label(text, style);
				}
			}
			GUILayout.EndScrollView();
		}

		private static GUIStyle GetStyleForEntry(int index)
		{
			GUIStyle style;
			if (index % 2 == 0)
			{
				style = Styles.EvenEntry;
			}
			else
			{
				style = Styles.OddEntry;
			}

			return style;
		}

		/// <summary>
		/// Get the editor height of the complete list with all entries
		/// </summary>
		/// <returns></returns>
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