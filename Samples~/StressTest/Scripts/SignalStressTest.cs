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

namespace Supyrb
{
	public class SignalStressTest : MonoBehaviour
	{
		[SerializeField]
		private int numListener = 10000;

		[SerializeField]
		private bool dispatchEveryUpdate = true;

		private List<DebugListener> listeners = null;
		
		private StressTestSignal signal;
		private void Awake()
		{
			Signals.Get(out signal);
			
			listeners = new List<DebugListener>();
			for (int i = 0; i < numListener; i++)
			{
				listeners.Add(new DebugListener(i, signal));
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

		private void SubscribeListeners()
		{
			for (int i = 0; i < listeners.Count; i++)
			{
				var listener = listeners[i];
				listener.Subscribe();
			}
		}
		
		private void UnsubscribeListeners()
		{
			for (int i = 0; i < listeners.Count; i++)
			{
				var listener = listeners[i];
				listener.Unsubscribe();
			}
		}
		
		private void OnDestroy()
		{
			UnsubscribeListeners();
		}
		
		[ContextMenu("DispatchSignal")]
		public void DispatchSignal()
		{
			Debug.LogFormat("Dispatching stress signal with {0} listeners", signal.ListenerCount);
			signal.Dispatch();
		}
	}
}