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
		private List<SerializableSystemType> signalTypes = null;

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

		[InitializeOnLoadMethod]
		private static void FindOrCreateInstance()
		{
			if (instance != null)
			{
				return;
			}

			instance = SignalsEditorAssetUtilities.FindOrCreateEditorAsset<SignalsEditorDatabase>("Signals", "SignalsEditorDatabase.asset", false);
		}

		[ContextMenu("UpdateDatabase")]
		public void UpdateDatabase()
		{
			var types = new List<Type>();
			SignalReflectionHelper.GetAllDerivedClasses<ABaseSignal>(ref types);
			signalTypes.Clear();
			for (int i = 0; i < types.Count; i++)
			{
				var type = types[i];
				signalTypes.Add(new SerializableSystemType(type));
			}
		}
	}
}