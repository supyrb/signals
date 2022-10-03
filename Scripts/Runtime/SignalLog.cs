// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalLog.cs" company="Supyrb">
//   Copyright (c) 2020 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Supyrb
{
	public class SignalLog
	{
		public delegate void LogDelegate(SignalLogEntry logEntry);
		public event LogDelegate OnNewSignalLog;

		private bool subscribed;
		private readonly List<SignalLogEntry> log;
		private readonly Dictionary<Type, SignalLogEntry> lastDispatch;

		private static SignalLog _instance;

		public static SignalLog Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new SignalLog();
				}

				return _instance;
			}
		}

		private SignalLog()
		{
			log = new List<SignalLogEntry>();
			lastDispatch = new Dictionary<Type, SignalLogEntry>();
			subscribed = false;
		}

		public void Subscribe()
		{
			if (subscribed)
			{
				return;
			}
			
			Signals.OnSignalDispatch += OnSignalDispatch;
			subscribed = true;
		}

		public void Unsubscribe()
		{
			if (!subscribed)
			{
				return;
			}

			Signals.OnSignalDispatch -= OnSignalDispatch;
			subscribed = false;
		}
		
		public SignalLogEntry GetLastOccurenceOf(Type type)
		{
			return lastDispatch.TryGetValue(type, out  SignalLogEntry item) ? item : null;
		}

		public SignalLogEntry GetLastEntry()
		{
			if (log.Count == 0)
			{
				return null;
			}

			return log[log.Count - 1];
		}

		public void Clear()
		{
			log.Clear();
			lastDispatch.Clear();
		}

		private void OnSignalDispatch(ASignal signal, string memberName, string sourceFilePath, int sourceLineNumber)
		{
			var signalLogItem = new SignalLogEntry(signal, memberName, sourceFilePath, sourceLineNumber);
			log.Add(signalLogItem);
			lastDispatch[signalLogItem.SignalType.SystemType] = signalLogItem;

			if (OnNewSignalLog != null)
			{
				OnNewSignalLog(signalLogItem);
			}
		}

		public bool GetLogEntriesForType(Type type, ref List<SignalLogEntry> signalLog)
		{
			if (!lastDispatch.ContainsKey(type))
			{
				return false;
			}

			var lastLogEntry = lastDispatch[type];
			SignalLogEntry lastListEntry = null;
			if (signalLog.Count > 0)
			{
				lastListEntry = signalLog[signalLog.Count - 1];
			}

			if (lastListEntry == lastLogEntry)
			{
				return false;
			}

			var startProcessingIndex = 0;
			if (lastListEntry != null)
			{
				startProcessingIndex = log.IndexOf(lastListEntry) + 1;
				Assert.IsTrue(startProcessingIndex > 0);
			}

			for (var i = startProcessingIndex; i < log.Count; i++)
			{
				var entry = log[i];
				if (entry.SignalType.SystemType == type)
				{
					signalLog.Add(entry);
				}
			}

			return true;
		}
	}
}