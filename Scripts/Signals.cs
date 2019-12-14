// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Signals.cs" company="Supyrb">
//   Copyright (c) 2019 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// <author>
//   Yanko Oliveira
//   https://github.com/yankooliveira
// </author>
// <summary>
// Inspired by
// Signals by Yanko Oliveira
// https://github.com/yankooliveira/signals
// and
// JS-Signas by Miller Medeiros
// https://github.com/millermedeiros/js-signals
// </summary
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Supyrb
{
	/// <summary>
	/// Global Signal hub
	/// </summary>
	public static class Signals
	{
		private static readonly SignalHub signalHub = new SignalHub();

		/// <summary>
		/// Get the Signal for a certain class. If the signal is not yet registered in the hub it will be created.
		/// </summary>
		/// <typeparam name="T">Type of the Signal to retrieve</typeparam>
		/// <returns>An instance of the Signal of SType</returns>
		public static T Get<T>() where T : ISignal, new()
		{
			return signalHub.Get<T>();
		}
		
		/// <summary>
		/// Get the Signal for a certain class. If the signal is not yet registered in the hub it will be created.
		/// </summary>
		/// <param name="reference">The output argument for which the reference will be set</param>
		/// <typeparam name="T">The signal type to retrieve</typeparam>
		public static void Get<T>(out T reference) where T : ISignal, new()
		{
			reference = Get<T>();
		}
		
		/// <summary>
		/// The number of registered signals
		/// </summary>
		public static int Count
		{
			get { return signalHub.Count; }
		}

		/// <summary>
		/// Removes all registered signals
		/// </summary>
		public static void Clear()
		{
			signalHub.Clear();
		}
	}

	public class SignalHub
	{
		private readonly Dictionary<Type, ISignal> signals = new Dictionary<Type, ISignal>();

		/// <summary>
		/// Getter for a signal of a given type
		/// </summary>
		/// <typeparam name="T">Type of signal</typeparam>
		/// <returns>The proper signal binding</returns>
		public T Get<T>() where T : ISignal, new()
		{
			var signalType = typeof(T);
			ISignal signal;

			if (signals.TryGetValue(signalType, out signal))
			{
				return (T) signal;
			}

			return (T) Bind(signalType);
		}
		
		/// <summary>
		/// The number of registered signals in the hub
		/// </summary>
		public int Count
		{
			get { return signals.Count; }
		}

		/// <summary>
		/// Removes all signals from the hub
		/// </summary>
		public void Clear()
		{
			signals.Clear();
		}

		private ISignal Bind(Type signalType)
		{
			ISignal signal;
			if (signals.TryGetValue(signalType, out signal))
			{
				UnityEngine.Debug.LogError(string.Format("Signal already registered for type {0}", signalType.ToString()));
				return signal;
			}

			signal = (ISignal) Activator.CreateInstance(signalType);
			signals.Add(signalType, signal);
			return signal;
		}

		private ISignal Bind<T>() where T : ISignal, new()
		{
			return Bind(typeof(T));
		}
	}
}