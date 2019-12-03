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
	public class DebugListener
	{
		private int order;
		private bool subscribed;
		private Signal signal = null;

		public DebugListener(int order, Signal signal)
		{
			this.order = order;
			this.signal = signal;
			this.subscribed = false;
		}

		public void Subscribe()
		{
			if (subscribed)
			{
				return;
			}
			if (order % 1000 == 0)
			{
				Debug.LogFormat("Subscribe Listener with order {0}", order);
			}
			signal.AddListener(OnSignal, order);
			subscribed = true;
		}
		
		public void Unsubscribe()
		{
			if (!subscribed)
			{
				return;
			}
			if (order % 1000 == 0)
			{
				Debug.LogFormat("Unsubscribe Listener with order {0}", order);
			}
			signal.RemoveListener(OnSignal);
			subscribed = false;
		}

		private void OnSignal()
		{
			if (order % 1000 == 0)
			{
				Debug.LogFormat("Listener with order {0} has received the signal", order);
			}
		}
	}
	
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
			signal = Signals.Get<StressTestSignal>();
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