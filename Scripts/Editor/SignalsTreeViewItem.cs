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
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace Supyrb
{
	public class SignalsTreeViewItem
	{
		static class Styles
		{
			internal static GUIStyle HeaderLabel;

			static Styles()
			{
				HeaderLabel = new GUIStyle((GUIStyle)"AM MixerHeader");
				HeaderLabel.margin.left = 4;
			}
		}

		private const int NumDispatchDefaultArguments = 3;
		private readonly Type type;
		private readonly Type baseType;
		private readonly Type[] argumentTypes;

		private ASignal instance;
		private object[] argumentValues;
		private MethodInfo dispatchMethod;
		private FieldInfo currentIndexField;
		private FieldInfo stateField;
		private int currentIndex;
		private ASignal.State state;
		
		private SignalLogViewDrawer logViewDrawer;
		private SignalListenerViewDrawer listenerViewDrawer;

		private bool foldoutListeners = true;
		private bool foldoutLog = true;

		public ASignal Instance
		{
			get { return instance; }
		}

		public int CurrentIndex
		{
			get { return currentIndex; }
		}

		public ASignal.State State
		{
			get { return state; }
		}

		public SignalsTreeViewItem(Type type)
		{
			this.type = type;

			baseType = this.type.BaseType;
			if (baseType == null || baseType == typeof(ASignal))
			{
				baseType = this.type;
			}

			argumentTypes = baseType.GetGenericArguments();
			argumentValues = new object[argumentTypes.Length + NumDispatchDefaultArguments];


			dispatchMethod = this.type.GetMethod("Dispatch", BindingFlags.Instance | BindingFlags.Public);
			currentIndexField = typeof(ASignal).GetField("currentIndex", BindingFlags.Instance | BindingFlags.NonPublic);
			stateField = typeof(ASignal).GetField("state", BindingFlags.Instance | BindingFlags.NonPublic);

			logViewDrawer = new SignalLogViewDrawer(type);
			listenerViewDrawer = new SignalListenerViewDrawer(this, baseType);
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
			GUILayout.Label(string.Format(type.Name), Styles.HeaderLabel);
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
				DispatchSignalFromEditor();
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

		private void DispatchSignalFromEditor()
		{
			DispatchSignalViaReflection();
		}

		private void DispatchSignalViaReflection([CallerMemberName] string memberName = "",
													[CallerFilePath] string sourceFilePath = "",
													[CallerLineNumber] int sourceLineNumber = 0)
		{
			argumentValues[argumentTypes.Length] = memberName;
			argumentValues[argumentTypes.Length + 1] = sourceFilePath;
			argumentValues[argumentTypes.Length + 2] = sourceLineNumber;
			dispatchMethod.Invoke(instance, argumentValues);
		}

		private void DrawLogs()
		{
			foldoutLog = EditorGUILayout.Foldout(foldoutLog, "Log");
			if (!foldoutLog)
			{
				return;
			}
			logViewDrawer.Update();
			logViewDrawer.DrawLogsForCurrentType();
		}

		private void DrawListeners()
		{
			foldoutListeners = EditorGUILayout.Foldout(foldoutListeners, $"Listeners ({instance.ListenerCount})");
			if (!foldoutListeners)
			{
				return;
			}
			
			listenerViewDrawer.DrawListeners();
		}
		
		public void Reset()
		{
			instance = null;
			logViewDrawer.Reset();
		}
	}
}