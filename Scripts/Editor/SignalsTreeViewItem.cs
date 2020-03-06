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
		static class Styles {
			
			internal static GUIStyle HeaderLabel;
			internal static GUIStyle NumberLabel;
			
			static Styles()
			{
				HeaderLabel = (GUIStyle)"AM MixerHeader";
				HeaderLabel.margin.left = 4;
				NumberLabel = new GUIStyle(EditorStyles.label);
				NumberLabel.alignment = TextAnchor.MiddleRight;
				NumberLabel.fixedWidth = 50f;
				NumberLabel.padding.right = 8;
			}
		}
		
		private readonly Type type;
		private readonly Type baseType;
		private readonly Type[] argumentTypes;
		private ASignal instance;
		private object[] argumentValues;
		private MethodInfo dispatchMethod;
		private FieldInfo listenersField;
		private bool foldoutListeners = true;

		public SignalsTreeViewItem(Type type)
		{
			this.type = type;

			baseType = this.type.BaseType;
			if (baseType == null || baseType == typeof(ASignal))
			{
				baseType = this.type;
			}

			argumentTypes = baseType.GetGenericArguments();
			argumentValues = new object[argumentTypes.Length];

			dispatchMethod = this.type.GetMethod("Dispatch", BindingFlags.Instance | BindingFlags.Public);
			listenersField = baseType.GetField("listeners", BindingFlags.Instance | BindingFlags.NonPublic);
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

			GUILayout.Label(string.Format(type.Name), Styles.HeaderLabel);
			GUILayout.Label(string.Format("Current State: {0}",
				instance.GetCurrentState()));
			GUILayout.Space(12f);

			for (var i = 0; i < argumentTypes.Length; i++)
			{
				var argumentType = argumentTypes[i];
				var argumentValue = argumentValues[i];
				argumentValues[i] = DrawField(argumentType.Name, argumentType, argumentValue);
			}

			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Dispatch"))
			{
				dispatchMethod.Invoke(instance, argumentValues);
			}
			if (GUILayout.Button("Consume"))
			{
				instance.Consume();
			}
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Continue"))
			{
				instance.Continue();
			}
			
			if (GUILayout.Button("Pause"))
			{
				instance.Pause();
			}
			GUILayout.EndHorizontal();

			GUILayout.Space(24f);
			DrawListeners();
			
			GUILayout.EndVertical();
		}

		public void ResetInstance()
		{
			instance = null;
		}
		
		private void DrawListeners()
		{
			if (instance.ListenerCount == 0)
			{
				GUILayout.Label("No listeners subscribed");
				return;
			}

			foldoutListeners = EditorGUILayout.Foldout(foldoutListeners, string.Format("Listeners ({0})", instance.ListenerCount));
			if (!foldoutListeners)
			{
				return;
			}
			dynamic listeners = listenersField.GetValue(instance);
			
			for (int i = 0; i < listeners.Count; i++)
			{
				int sortOrder = listeners.GetSortOrderForIndex(i);
				var listener = listeners[i];
				var target = listener.Target;
				Type targetType = target.GetType();
				
				GUILayout.BeginHorizontal();
				
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

		private object DrawField(string label, Type type, object value)
		{
			if (typeof(UnityEngine.Object).IsAssignableFrom(type))
			{
				return EditorGUILayout.ObjectField(label, (UnityEngine.Object) value, type, true);
			}

			if (type == typeof(string))
			{
				return EditorGUILayout.TextField(label, (string) value);
			}
			
			if (type == typeof(AnimationCurve))
			{
				return EditorGUILayout.CurveField(label, (AnimationCurve) value);
			}

			if (type == typeof(Gradient))
			{
				return EditorGUILayout.GradientField(label, (Gradient) value);
			}
			
			if (type.IsEnum)
			{
				int enumValue = 0;
				if (value != null)
				{
					enumValue = (int) value;
				}
				return EditorGUILayout.EnumPopup(label, (Enum) Enum.ToObject(type, enumValue));
			}
			
			if (type == typeof(bool))
			{
				var boolValue = false;
				if (value != null)
				{
					boolValue = (bool) value;
				}
				return EditorGUILayout.Toggle(label, boolValue);
			}

			if (type == typeof(int))
			{
				var intValue = 0;
				if (value != null)
				{
					intValue = (int) value;
				}
				return EditorGUILayout.IntField(label, (int) intValue);
			}

			if (type == typeof(long))
			{
				var longValue = 0L;
				if (value != null)
				{
					longValue = (long) value;
				}
				return EditorGUILayout.LongField(label, longValue);
			}

			if (type == typeof(float))
			{
				var floatValue = 0f;
				if (value != null)
				{
					floatValue = (float) value;
				}
				return EditorGUILayout.FloatField(label, floatValue);
			}

			if (type == typeof(Vector2))
			{
				var vectorValue = Vector2.zero;
				if (value != null)
				{
					vectorValue = (Vector2) value;
				}
				return EditorGUILayout.Vector2Field(label, vectorValue);
			}

			if (type == typeof(Vector3))
			{
				var vectorValue = Vector3.zero;
				if (value != null)
				{
					vectorValue = (Vector3) value;
				}
				return EditorGUILayout.Vector3Field(label, vectorValue);
			}

			if (type == typeof(Vector4))
			{
				var vectorValue = Vector4.zero;
				if (value != null)
				{
					vectorValue = (Vector4) value;
				}
				return EditorGUILayout.Vector4Field(label, vectorValue);
			}

			if (type == typeof(Color))
			{
				var colorValue = Color.black;
				if (value != null)
				{
					colorValue = (Color) value;
				}
				return EditorGUILayout.ColorField(label, colorValue);
			}

			GUILayout.Label(string.Format("Type {0} not supported", type.Name));
			return null;
		}
	}
}