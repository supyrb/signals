// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalsEditorUtilities.cs" company="Supyrb">
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
using UnityEngine.Assertions;

namespace Supyrb
{
	public static class SignalsEditorUtilities
	{
		/// <summary>
		/// Get or Create a ScriptabelObject in the EditorDefaultResources folder.
		/// </summary>
		/// <typeparam name="T">Type of the scriptable object</typeparam>
		/// <param name="folderPath">Path to the asset relative to the EditorDefaultResources folder. e.g. QualityAssurace/Materials</param>
		/// <param name="fileName">Name of the file including the extension, e.g. MaterialCollector.asset</param>
		/// <param name="searchOutsideResources">Whether the file should be searched in the complete project if no asset is found at the defined location</param>
		/// <returns>The found or created asset</returns>
		public static T FindOrCreateEditorAsset<T>(string folderPath, string fileName, bool searchOutsideResources) where T : ScriptableObject
		{
			if (folderPath == null)
			{
				folderPath = string.Empty;
			}

			var asset = EditorGUIUtility.Load(Path.Combine(folderPath, fileName)) as T;
			if (asset == null && searchOutsideResources)
			{
				var guids = AssetDatabase.FindAssets("t:" + typeof(T).FullName);
				if (guids.Length > 0)
				{
					if (guids.Length > 1)
					{
						Debug.LogWarningFormat("More than one Asset of the type {0} exists:", typeof(T).FullName);
						for (var i = 0; i < guids.Length; i++)
						{
							var path = AssetDatabase.GUIDToAssetPath(guids[i]);
							var assetAtPath = AssetDatabase.LoadAssetAtPath<T>(path);
							Debug.Log(path, assetAtPath);
						}
					}

					var pathToFirstAsset = AssetDatabase.GUIDToAssetPath(guids[0]);
					asset = AssetDatabase.LoadAssetAtPath<T>(pathToFirstAsset);
				}
			}

			if (asset == null)
			{
				asset = ScriptableObject.CreateInstance<T>();
				var assetRelativeFolderPath = "Assets/Editor Default Resources/" + folderPath;
				// Create folders if not not existent
				Directory.CreateDirectory(Path.GetFullPath(assetRelativeFolderPath));
				var assetRelativeFilePath = Path.Combine(assetRelativeFolderPath, fileName);
				AssetDatabase.CreateAsset(asset, assetRelativeFilePath);
			}

			return asset;
		}
		
		/// <summary>
		/// Creates a new Texture2D with 1x1 pixels and a defined color.
		/// Easy solution for editor style background textures that should just be a color
		/// </summary>
		/// <param name="col">The color for the new texture</param>
		/// <returns>A new 1x1 texture with the defined color</returns>
		public static Texture2D CreateColorTexture(Color col)
		{
			Texture2D result = new Texture2D(1, 1);
			result.SetPixel(0, 0, col);
			result.Apply();

			return result;
		}
		
		/// <summary>
		/// Modify the color of a 1x1 texture
		/// </summary>
		/// <param name="tex">Texture to modify</param>
		/// <param name="col">New color</param>
		public static void ChangeColorTexture(Texture2D tex, Color col)
		{
			Assert.IsTrue(tex.width == 1);
			Assert.IsTrue(tex.height == 1);
			
			tex.SetPixel(0, 0, col);
			tex.Apply();
		}

		/// <summary>
		/// Similar to <see cref="EditorGUILayout.ObjectField(UnityEngine.Object,System.Type,UnityEngine.GUILayoutOption[])"/>, but more flexible.
		/// Draws an editor field fitting to the type and returns the user value for that field.
		/// Supports
		/// * <see cref="UnityEngine.Object"/>
		/// * <see cref="AnimationCurve"/>
		/// * <see cref="Gradient"/>
		/// * <see cref="string"/>
		/// * <see cref="Enum"/>
		/// * <see cref="bool"/>
		/// * <see cref="int"/>
		/// * <see cref="long"/>
		/// * <see cref="float"/>
		/// * <see cref="UnityEngine.Vector2"/>
		/// * <see cref="UnityEngine.Vector3"/>
		/// * <see cref="UnityEngine.Vector4"/>
		/// * <see cref="UnityEngine.Color"/>
		/// </summary>
		/// <param name="label">Optional label in front of the field</param>
		/// <param name="type">Type of the value</param>
		/// <param name="value">Current value for that field</param>
		/// <returns>The new value set by the user</returns>
		public static object DrawFittingEditorField(string label, Type type, object value)
		{
			if (typeof(UnityEngine.Object).IsAssignableFrom(type))
			{
				return EditorGUILayout.ObjectField(label, (UnityEngine.Object) value, type, true);
			}

			if (type == typeof(AnimationCurve))
			{
				return EditorGUILayout.CurveField(label, (AnimationCurve) value);
			}

			if (type == typeof(Gradient))
			{
				return EditorGUILayout.GradientField(label, (Gradient) value);
			}

			if (type == typeof(string))
			{
				return EditorGUILayout.TextField(label, (string) value);
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

			GUILayout.Label(string.Format("Field Type {0} not supported", type.FullName));
			return null;
		}
	}
}