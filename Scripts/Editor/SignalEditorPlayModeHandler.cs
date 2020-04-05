// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalEditorPlayModeHandler.cs" company="Supyrb">
//   Copyright (c) 2020 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace Supyrb
{
	/// <summary>
	/// Handler for exiting Playmode
	/// Resets the Signal hub to make sure the next Playmode will not reuse old signals.
	/// This is necessary for supporting the experimental enter Playmode introduced in Unity 2019.3
	/// </summary>
	public static class SignalEditorPlayModeHandler
	{
		[InitializeOnLoadMethod]
		private static void OnProjectLoadedInEditor()
		{
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
		}

		private static void OnPlayModeStateChanged(PlayModeStateChange state)
		{
			if (state == PlayModeStateChange.EnteredEditMode)
			{
				OnEnterEditMode();
			}
		}

		private static void OnEnterEditMode()
		{
			var resetSignals = EditorPrefs.GetBool("ResetSignalsInEditmode", true);
			if (resetSignals)
			{
				ResetSignalHub();
			}
		}

		private static void ResetSignalHub()
		{
			Debug.LogFormat("Resetting signal hub with {0} registered signals", Signals.Count);
			Signals.Clear();
		}
	}
}