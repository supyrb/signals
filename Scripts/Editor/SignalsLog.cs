// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalsLog.cs" company="Supyrb">
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

	public class SignalsLog
	{
		public delegate void LogDelegate(SignalLogItem logItem);

		public event LogDelegate OnNewSignalLog;

		private readonly List<SignalLogItem> log;
		private readonly Dictionary<Type, SignalLogItem> lastDispatch;

		public SignalsLog()
		{
			log = new List<SignalLogItem>();
			lastDispatch = new Dictionary<Type, SignalLogItem>();
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

		public void Subscribe()
		{
			Signals.OnSignalDispatch += OnSignalDispatch;
		}

		public void Unsubscribe()
		{
			Signals.OnSignalDispatch -= OnSignalDispatch;
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
	}
}