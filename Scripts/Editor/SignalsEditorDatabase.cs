// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalsEditorDatabase.cs" company="Supyrb">
//   Copyright (c) 2020 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Supyrb
{
	public class SignalsEditorDatabase : ScriptableObject
	{
		[SerializeField]
		private List<SerializableSystemType> signalTypes = new List<SerializableSystemType>();

		public List<SerializableSystemType> SignalTypes
		{
			get
			{
				return signalTypes;
			}
		}

		private static SignalsEditorDatabase instance = null;

		public static SignalsEditorDatabase Instance
		{
			get
			{
				if (instance == null)
				{
					FindOrCreateInstance();
				}

				return instance;
			}
		}

		private static void FindOrCreateInstance()
		{
			if (instance != null)
			{
				return;
			}

			instance = SignalsEditorUtilities.FindOrCreateEditorAsset<SignalsEditorDatabase>("Signals", "SignalsEditorDatabase.asset", false);
		}

		[ContextMenu("UpdateDatabase")]
		public void UpdateDatabase()
		{
			try
			{
				EditorUtility.DisplayProgressBar("Update Signals List", "Find all signals in project", 0.1f);
				var types = new List<Type>();
				SignalReflectionHelper.GetAllDerivedClasses<ABaseSignal>(ref types);
				signalTypes.Clear();
				EditorUtility.DisplayProgressBar("Update Signals List", string.Format("Serialize found signals ({0})", types.Count), 0.6f);
				for (int i = 0; i < types.Count; i++)
				{
					var type = types[i];
					signalTypes.Add(new SerializableSystemType(type));
				}
				signalTypes.Sort();
			
				EditorUtility.DisplayProgressBar("Update Signals List", string.Format("Store found signals ({0})", types.Count), 0.9f);
				EditorUtility.SetDirty(this);
				AssetDatabase.SaveAssets();
			}
			finally
			{
				EditorUtility.ClearProgressBar();
			}

		}
	}
}