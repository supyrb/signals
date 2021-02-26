// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DebugListener.cs" company="Supyrb">
//   Copyright (c) 2019 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using UnityEngine;

namespace Supyrb
{
	public class DebugListener
	{
		private readonly int order;
		private bool subscribed;
		private bool verbose;
		private readonly Signal signal;
		
		// Cache action in order to avoid additional garbage every subscribe/unsubscribe
		private readonly Action onSignalAction;

		public DebugListener(int order, Signal signal, bool verbose)
		{
			this.order = order;
			this.signal = signal;
			this.verbose = verbose;
			this.onSignalAction = OnSignal;
			this.subscribed = false;
		}

		public void Subscribe()
		{
			if (subscribed)
			{
				return;
			}

			if (verbose && order % 1000 == 0)
			{
				Debug.LogFormat("Subscribe Listener with order {0}", order);
			}

			signal.AddListener(onSignalAction, order);
			subscribed = true;
		}

		public void Unsubscribe()
		{
			if (!subscribed)
			{
				return;
			}

			if (verbose && order % 1000 == 0)
			{
				Debug.LogFormat("Unsubscribe Listener with order {0}", order);
			}

			signal.RemoveListener(onSignalAction);
			subscribed = false;
		}

		private void OnSignal()
		{
			if (verbose && order % 1000 == 0)
			{
				Debug.LogFormat("Listener with order {0} has received the signal", order);
			}
		}
	}
}