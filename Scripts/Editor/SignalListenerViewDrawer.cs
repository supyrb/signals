// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalListenerViewDrawer.cs" company="Supyrb">
//   Copyright (c) 2020 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Supyrb
{
	public class SignalListenerViewDrawer
	{
		private static class Styles
		{
			internal static GUIStyle NumberLabel;

			internal static GUIStyle RunningLabel;
			internal static GUIStyle PausedLabel;
			internal static GUIStyle ConsumedLabel;

			static Styles()
			{
				NumberLabel = new GUIStyle(EditorStyles.label);
				NumberLabel.alignment = TextAnchor.MiddleRight;
				NumberLabel.fixedWidth = 50f;
				NumberLabel.padding.right = 8;

				RunningLabel = new GUIStyle(EditorStyles.label);
				RunningLabel.normal.background = SignalsEditorUtilities.CreateColorTexture(new Color(0.2f, 0.8f, 0.2f, 0.4f));
				PausedLabel = new GUIStyle(EditorStyles.label);
				PausedLabel.normal.background = SignalsEditorUtilities.CreateColorTexture(new Color(0.8f, 0.8f, 0.2f, 0.6f));
				ConsumedLabel = new GUIStyle(EditorStyles.label);
				ConsumedLabel.normal.background = SignalsEditorUtilities.CreateColorTexture(new Color(0.8f, 0.2f, 0.2f, 0.4f));
			}
		}

		private FieldInfo listenersField;
		private SignalsTreeViewItem parent;

		public SignalListenerViewDrawer(SignalsTreeViewItem parent, Type baseType)
		{
			this.parent = parent;
			listenersField = baseType.GetField("listeners", BindingFlags.Instance | BindingFlags.NonPublic);
		}

		public void DrawListeners()
		{
			if (parent.Instance.ListenerCount == 0)
			{
				GUILayout.Label("No listeners subscribed");
				return;
			}

			dynamic listeners = listenersField.GetValue(parent.Instance);

			for (var i = 0; i < listeners.Count; i++)
			{
				int sortOrder = listeners.GetSortOrderForIndex(i);
				var listener = listeners[i];
				var target = listener.Target;
				Type targetType = target.GetType();

				var style = GetStyleForIndex(i);
				GUILayout.BeginHorizontal(style);

				GUILayout.Label(i.ToString(), GUILayout.Width(30f));
				GUILayout.Label(GetSortOrderString(sortOrder), Styles.NumberLabel);
				if (typeof(UnityEngine.Object).IsAssignableFrom(targetType))
				{
					EditorGUILayout.ObjectField((UnityEngine.Object) target, targetType, true);
				}
				else
				{
					GUILayout.Label(target.ToString());
				}

				GUILayout.Label("▶ " + listener.Method.Name);
				GUILayout.FlexibleSpace();

				GUILayout.EndHorizontal();
			}
		}

		private GUIStyle GetStyleForIndex(int index)
		{
			if (parent.CurrentIndex != index || parent.State == ASignal.State.Idle)
			{
				return EditorStyles.label;
			}

			switch (parent.State)
			{
				case ASignal.State.Running:
					return Styles.RunningLabel;
				case ASignal.State.Paused:
					return Styles.PausedLabel;
				case ASignal.State.Consumed:
					return Styles.ConsumedLabel;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private string GetSortOrderString(int sortOrder)
		{
			if (sortOrder == Int32.MinValue)
			{
				return "min";
			}

			if (sortOrder == Int32.MaxValue)
			{
				return "max";
			}

			return sortOrder.ToString();
		}
	}
}