// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditorSignalLog.cs">
//   Copyright (c) 2022 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Supyrb
{
	public class EditorSignalLog
	{
		[Flags]
		public enum ConsoleFlags
		{
			Collapse = 1 << 0,
			ClearOnPlay = 1 << 1,
			ClearOnBuild = 1 << 2,
			ClearOnRecompile = 1 << 3,
		}

		public Action OnClearLogs;
		public event SignalLog.LogDelegate OnNewSignalLog;
		
		private static EditorSignalLog _instance;
		private const string ConsoleFlagsEditorKey = "SignalConsoleFlags";
		private int numSubscribers;
		private ConsoleFlags consoleFlags;

		public static EditorSignalLog Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new EditorSignalLog();
				}

				return _instance;
			}
		}
		
		[DidReloadScripts]
		private static void OnScriptsReloaded() {
			if (_instance != null)
			{
				Instance.OnRecompile();
			}
		}
		
		[PostProcessBuild(1)]
		public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
			if (_instance != null)
			{
				Instance.OnBuildFinished();
			}
		}

		private EditorSignalLog()
		{
			numSubscribers = 0;
			consoleFlags = (ConsoleFlags)EditorPrefs.GetInt(ConsoleFlagsEditorKey, (int)ConsoleFlags.ClearOnPlay);
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
			
		}

		~EditorSignalLog()
		{
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
		}

		public void Subscribe()
		{
			if (numSubscribers == 0)
			{
				SignalLog.Instance.Subscribe();
				SignalLog.Instance.OnNewSignalLog += OnDispatchNewSignalLog;
			}

			numSubscribers++;
		}

		public void Unsubscribe()
		{
			numSubscribers--;
			if (numSubscribers == 0)
			{
				SignalLog.Instance.Unsubscribe();
				SignalLog.Instance.OnNewSignalLog -= OnDispatchNewSignalLog;
			}
		}
		
		public bool HasFlag(ConsoleFlags flags)
		{
			return (consoleFlags & flags) != 0;
		}

		public void SetFlag(ConsoleFlags flags, bool val)
		{
			if (val)
			{
				consoleFlags |= flags;
			}
			else
			{
				consoleFlags &= ~flags;
			}
			
			EditorPrefs.SetInt(ConsoleFlagsEditorKey, (int)consoleFlags);
		}
		
		private void OnPlayModeStateChanged(PlayModeStateChange state)
		{
			if (state == PlayModeStateChange.ExitingEditMode)
			{
				if (HasFlag(ConsoleFlags.ClearOnPlay))
				{
					Clear();
				}

				SignalLog.Instance.Unsubscribe();
				SignalLog.Instance.Subscribe();
			}
		}
		
		private void OnRecompile()
		{
			if (HasFlag(ConsoleFlags.ClearOnRecompile))
			{
				Clear();
			}
		}
		
		private void OnBuildFinished()
		{
			if (HasFlag(ConsoleFlags.ClearOnBuild))
			{
				Clear();
			}
		}
		
		private void OnDispatchNewSignalLog(SignalLogEntry logEntry)
		{
			OnNewSignalLog?.Invoke(logEntry);
		}
		
		private void Clear()
		{
			SignalLog.Instance.Clear();
			OnClearLogs?.Invoke();
		}
	}
}