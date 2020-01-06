// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalsEditorAssetUtilities.cs" company="Supyrb">
//   Copyright (c) 2020 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;
using UnityEditor;
using UnityEngine;

namespace Supyrb
{
	public static class SignalsEditorAssetUtilities
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
	}
}