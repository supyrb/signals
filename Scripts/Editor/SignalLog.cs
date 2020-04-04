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
using UnityEngine;
using UnityEngine.Assertions;

namespace Supyrb
{
	public class SignalLogItem
	{
		public readonly DateTime TimeStamp;
		public readonly float PlayDispatchTime;
		public readonly ASignal SignalInstance;
		public readonly Type SignalType;

		public SignalLogItem(ASignal signalInstance)
		{
			TimeStamp = DateTime.Now;
			PlayDispatchTime = Time.time;
			SignalInstance = signalInstance;
			SignalType = signalInstance.GetType();
		}
	}

	public class SignalLog
	{
		public delegate void LogDelegate(SignalLogItem logItem);

		public event LogDelegate OnNewSignalLog;

		private bool subscribed;
		private readonly List<SignalLogItem> log;
		private readonly Dictionary<Type, SignalLogItem> lastDispatch;

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
			log = new List<SignalLogItem>();
			lastDispatch = new Dictionary<Type, SignalLogItem>();
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
		
		public SignalLogItem GetLastOccurenceOf(Type type)
		{
			SignalLogItem item;
			if (lastDispatch.TryGetValue(type, out item))
			{
				return item;
			}

			return null;
		}

		public SignalLogItem GetLastEntry()
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

		private void OnSignalDispatch(ASignal signal)
		{
			var signalLogItem = new SignalLogItem(signal);
			log.Add(signalLogItem);
			lastDispatch[signalLogItem.SignalType] = signalLogItem;

			if (OnNewSignalLog != null)
			{
				OnNewSignalLog(signalLogItem);
			}
		}

		public bool UpdateLog(Type type, ref List<SignalLogItem> signalLog)
		{
			if (!lastDispatch.ContainsKey(type))
			{
				return false;
			}

			var lastLogEntry = lastDispatch[type];
			SignalLogItem lastListEntry = null;
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
				if (entry.SignalType == type)
				{
					signalLog.Add(entry);
				}
			}

			return true;
		}
	}
}