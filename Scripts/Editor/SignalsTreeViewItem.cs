// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalsTreeViewItem.cs" company="Supyrb">
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
	public class SignalsTreeViewItem
	{
		static class Styles
		{
			internal static readonly Color DispatchActive;
			internal static readonly Color DispatchInactive;
			
			internal static GUIStyle HeaderLabel;
			internal static GUIStyle NumberLabel;

			internal static GUIStyle DispatchIndicatorTemplate;
			internal static GUIStyle RunningLabel;
			internal static GUIStyle PausedLabel;
			internal static GUIStyle ConsumedLabel;

			static Styles()
			{
				DispatchActive = new Color(0.2f, 0.8f, 0.2f, 0.9f);
				DispatchInactive = new Color(0.5f, 0.5f, 0.5f, 0.9f);
				
				HeaderLabel = (GUIStyle)"AM MixerHeader";
				HeaderLabel.margin.left = 4;
				NumberLabel = new GUIStyle(EditorStyles.label);
				NumberLabel.alignment = TextAnchor.MiddleRight;
				NumberLabel.fixedWidth = 50f;
				NumberLabel.padding.right = 8;
				
				DispatchIndicatorTemplate = new GUIStyle(EditorStyles.miniLabel);
				DispatchIndicatorTemplate.fixedWidth = 8f;
				DispatchIndicatorTemplate.fixedHeight = 8f;
				DispatchIndicatorTemplate.margin.left = 8;
				DispatchIndicatorTemplate.margin.right = 8;
				DispatchIndicatorTemplate.margin.top = 8;
				DispatchIndicatorTemplate.margin.bottom = 8;

				RunningLabel = new GUIStyle(EditorStyles.label);
				RunningLabel.normal.background = SignalsEditorUtilities.CreateColorTexture(new Color(0.2f, 0.8f, 0.2f, 0.4f));
				PausedLabel = new GUIStyle(EditorStyles.label);
				PausedLabel.normal.background = SignalsEditorUtilities.CreateColorTexture(new Color(0.8f, 0.8f, 0.2f, 0.6f));
				ConsumedLabel = new GUIStyle(EditorStyles.label);
				ConsumedLabel.normal.background = SignalsEditorUtilities.CreateColorTexture(new Color(0.8f, 0.2f, 0.2f, 0.4f));
			}
		}

		private readonly Type type;
		private readonly Type baseType;
		private readonly Type[] argumentTypes;
		private ASignal instance;
		private object[] argumentValues;
		private MethodInfo dispatchMethod;
		private FieldInfo listenersField;
		private FieldInfo currentIndexField;
		private FieldInfo stateField;
		private int currentIndex;
		private ASignal.State state;
		
		private SignalLogItem lastDispatchLog;
		private SignalLogViewDrawer logViewDrawer;
		private Color dispatchLogIndicatorColor;
		private GUIStyle dispatchIndicator;

		private bool foldoutListeners = true;
		private bool foldoutLog = true;

		public SignalsTreeViewItem(Type type)
		{
			this.type = type;

			baseType = this.type.BaseType;
			if (baseType == null || baseType == typeof(ASignal))
			{
				baseType = this.type;
			}
			
			logViewDrawer = new SignalLogViewDrawer(type);
			
			argumentTypes = baseType.GetGenericArguments();
			argumentValues = new object[argumentTypes.Length];

			dispatchMethod = this.type.GetMethod("Dispatch", BindingFlags.Instance | BindingFlags.Public);
			listenersField = baseType.GetField("listeners", BindingFlags.Instance | BindingFlags.NonPublic);
			currentIndexField = typeof(ASignal).GetField("currentIndex", BindingFlags.Instance | BindingFlags.NonPublic);
			stateField = typeof(ASignal).GetField("state", BindingFlags.Instance | BindingFlags.NonPublic);
			dispatchIndicator = new GUIStyle(Styles.DispatchIndicatorTemplate);
			dispatchIndicator.normal.background = SignalsEditorUtilities.CreateColorTexture(Styles.DispatchInactive);
		}

		public void DrawSignalDetailView()
		{
			GUILayout.BeginVertical();

			if (instance == null)
			{
				instance = Signals.Get(type) as ASignal;

				if (instance == null)
				{
					GUILayout.Label("Only signals derived from ASignal supported");
					return;
				}
			}

			var indexObject = currentIndexField.GetValue(instance);
			currentIndex = indexObject is int ? (int) indexObject : 0;
			var stateObject = stateField.GetValue(instance);
			state = stateObject is ASignal.State ? (ASignal.State) stateObject : ASignal.State.Idle;
			lastDispatchLog = SignalLog.Instance.GetLastOccurenceOf(type);

			DrawHeader();
			GUILayout.Space(24f);

			DrawDispatchPropertyFields();
			DrawButtons();

			GUILayout.Space(24f);
			DrawLogs();
			
			GUILayout.Space(24f);
			DrawListeners();

			GUILayout.EndVertical();
		}

		private void DrawHeader()
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(string.Format(type.Name), Styles.HeaderLabel);

			DrawDispatchIndicator();

			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		private void DrawDispatchIndicator()
		{
			if (dispatchIndicator.normal.background == null)
			{
				dispatchIndicator.normal.background = SignalsEditorUtilities.CreateColorTexture(Styles.DispatchInactive);
			}
			var newDispatchLogColor = CalculateDispatchLogColor();

			if (newDispatchLogColor != dispatchLogIndicatorColor)
			{
				dispatchLogIndicatorColor = newDispatchLogColor;
				SignalsEditorUtilities.ChangeColorTexture(dispatchIndicator.normal.background, dispatchLogIndicatorColor);
			}

			GUILayout.Label(string.Empty, dispatchIndicator);
		}

		private Color CalculateDispatchLogColor()
		{
			if (lastDispatchLog == null || lastDispatchLog.PlayDispatchTime > Time.time +0.1f)
			{
				return Styles.DispatchInactive;
			}

			var t = Mathf.Clamp01((Time.time - lastDispatchLog.PlayDispatchTime) / 1f);
			return Color.Lerp(Styles.DispatchActive, Styles.DispatchInactive, t);
		}

		private void DrawDispatchPropertyFields()
		{
			for (var i = 0; i < argumentTypes.Length; i++)
			{
				var argumentType = argumentTypes[i];
				var argumentValue = argumentValues[i];
				argumentValues[i] = SignalsEditorUtilities.DrawFittingEditorField(argumentType.Name, argumentType, argumentValue);
			}
		}

		private void DrawButtons()
		{
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Dispatch"))
			{
				dispatchMethod.Invoke(instance, argumentValues);
			}

			GUI.enabled = state == ASignal.State.Running || state == ASignal.State.Paused;
			if (GUILayout.Button("Consume"))
			{
				instance.Consume();
			}

			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUI.enabled = state == ASignal.State.Paused;
			if (GUILayout.Button("Continue"))
			{
				instance.Continue();
			}

			GUI.enabled = state == ASignal.State.Running;
			if (GUILayout.Button("Pause"))
			{
				instance.Pause();
			}

			GUILayout.EndHorizontal();
			GUI.enabled = true;
		}

		private void DrawLogs()
		{
			foldoutLog = EditorGUILayout.Foldout(foldoutLog, "Log");
			if (!foldoutLog)
			{
				return;
			}
			logViewDrawer.Update();
			logViewDrawer.DrawLog();
		}

		public void Reset()
		{
			instance = null;
			logViewDrawer.Reset();
		}

		private void DrawListeners()
		{
			foldoutListeners = EditorGUILayout.Foldout(foldoutListeners, string.Format("Listeners ({0})", instance.ListenerCount));
			if (!foldoutListeners)
			{
				return;
			}
			
			if (instance.ListenerCount == 0)
			{
				GUILayout.Label("No listeners subscribed");
				return;
			}

			dynamic listeners = listenersField.GetValue(instance);

			for (int i = 0; i < listeners.Count; i++)
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
			if (currentIndex != index || state == ASignal.State.Idle)
			{
				return EditorStyles.label;
			}

			switch (state)
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