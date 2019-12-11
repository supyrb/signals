// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DebugListener.cs" company="Supyrb">
//   Copyright (c) 2019 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

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
}