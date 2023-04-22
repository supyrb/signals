// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ASignalAction.cs" company="Supyrb">
//   Copyright (c) 2023 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Supyrb
{
	public abstract class ASignalAction<TAction> : ASignal
	where TAction : Delegate
	{
		protected readonly OrderedList<TAction> listeners;

		/// <inheritdoc />
		public override int ListenerCount
		{
			get { return listeners.Count; }
		}

		public ASignalAction() : base()
		{
			this.listeners = new OrderedList<TAction>(true);
		}

		/// <inheritdoc />
		public override void Clear()
		{
			listeners.Clear();
		}

		/// <summary>
		/// Add a listener for that signal
		/// </summary>
		/// <param name="listener">Listener Method to call</param>
		/// <param name="order">Lower order values will be called first</param>
		/// <returns>
		/// True, if the listener was added successfully
		/// False, if the listener was already subscribed
		/// </returns>
		public bool AddListener(TAction listener, int order = 0)
		{
			#if UNITY_EDITOR
			UnityEngine.Debug.Assert(listener.Method.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false).Length == 0,
				"Adding anonymous delegates as Signal callbacks is not supported (you wouldn't be able to unregister them later).");
			#endif
			var index = listeners.Add(order, listener);
			if (index < 0)
			{
				return false;
			}

			AddListenerAt(index);
			return true;
		}

		/// <summary>
		/// Remove a listener from that signal
		/// </summary>
		/// <param name="listener">Subscribed listener method</param>
		/// <returns>
		/// True, if the signal was removed successfully
		/// False, if the listener was not subscribed
		/// </returns>
		public bool RemoveListener(TAction listener)
		{
			var index = listeners.Remove(listener);
			if (index < 0)
			{
				return false;
			}

			RemoveListenerAt(index);
			return true;
		}
	}
}