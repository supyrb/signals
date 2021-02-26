// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalStressTest.cs" company="Supyrb">
//   Copyright (c)  Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Supyrb
{
	public class SignalStressTest : MonoBehaviour
	{
		[SerializeField]
		private int numListener = 10000;

		[SerializeField]
		private bool dispatchEveryUpdate = true;

		[Tooltip("Toggle Debug Logging")]
		[SerializeField]
		private bool verbose = true;
		
		private List<DebugListener> listeners = null;
		
		private StressTestSignal signal;
		private void Awake()
		{
			Signals.Get(out signal);
			
			listeners = new List<DebugListener>();
			for (int i = 0; i < numListener; i++)
			{
				listeners.Add(new DebugListener(i, signal, verbose));
			}
			SubscribeListeners();
		}

		private void Update()
		{
			if (!dispatchEveryUpdate)
			{
				return;
			}
			
			DispatchSignal();
		}

		private void OnDestroy()
		{
			UnsubscribeListeners();
		}
		
		private void SubscribeListeners()
		{
			Profiler.BeginSample("SubscribeListeners");
			for (int i = 0; i < listeners.Count; i++)
			{
				var listener = listeners[i];
				listener.Subscribe();
			}
			Profiler.EndSample();
		}
		
		private void UnsubscribeListeners()
		{
			Profiler.BeginSample("UnsubscribeListeners");
			for (int i = 0; i < listeners.Count; i++)
			{
				var listener = listeners[i];
				listener.Unsubscribe();
			}
			Profiler.EndSample();
		}

		[ContextMenu("DispatchSignal")]
		public void DispatchSignal()
		{
			if (verbose)
			{
				Debug.LogFormat("Dispatching stress signal with {0} listeners", signal.ListenerCount);
			}
			
			Profiler.BeginSample("DispatchSignal");
			{
				signal.Dispatch();
			}
			Profiler.EndSample();
		}
	}
}