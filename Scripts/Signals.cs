// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Signals.cs" company="Supyrb">
//   Copyright (c) 2019 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
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
		/// <typeparam name="SType">Type of the Signal</typeparam>
		/// <returns>An instance of the Signal of SType</returns>
		public static SType Get<SType>() where SType : ISignal, new()
		{
			return signalHub.Get<SType>();
		}
	}

	public class SignalHub
	{
		private Dictionary<Type, ISignal> signals = new Dictionary<Type, ISignal>();

		/// <summary>
		/// Getter for a signal of a given type
		/// </summary>
		/// <typeparam name="SType">Type of signal</typeparam>
		/// <returns>The proper signal binding</returns>
		public SType Get<SType>() where SType : ISignal, new()
		{
			var signalType = typeof(SType);
			ISignal signal;

			if (signals.TryGetValue(signalType, out signal))
			{
				return (SType) signal;
			}

			return (SType) Bind(signalType);
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