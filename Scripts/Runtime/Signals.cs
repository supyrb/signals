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
using System.Diagnostics;
using UnityEngine.Profiling;

namespace Supyrb
{
	/// <summary>
	/// Global Signal hub
	/// </summary>
	public static class Signals
	{
		public delegate void SignalDelegate(ASignal signal, string memberName, string sourceFilePath, int sourceLineNumber);
		public static event SignalDelegate OnSignalDispatch;
		private static readonly SignalHub signalHub = new SignalHub();

		/// <summary>
		/// Get the Signal for a certain class. If the signal is not yet registered in the hub it will be created.
		/// </summary>
		/// <typeparam name="T">Type of the Signal to retrieve</typeparam>
		/// <returns>An instance of the Signal of type T</returns>
		public static T Get<T>() where T : ABaseSignal
		{
			return signalHub.Get<T>();
		}
		
		/// <summary>
		/// Get the Signal for a certain class. If the signal is not yet registered in the hub it will be created.
		/// </summary>
		/// <param name="signalType">Type of the Signal to retrieve</param>
		/// <returns>An instance of the Signal of type signalType</returns>
		public static ABaseSignal Get(Type signalType)
		{
			return signalHub.Get(signalType);
		}
		
		/// <summary>
		/// Get the Signal for a certain class. If the signal is not yet registered in the hub it will be created.
		/// </summary>
		/// <param name="reference">The output argument for which the reference will be set</param>
		/// <typeparam name="T">The signal type to retrieve</typeparam>
		public static void Get<T>(out T reference) where T : ABaseSignal
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
			OnSignalDispatch = null;
		}

		[Conditional("SIGNALS_DEBUG"), Conditional("UNITY_EDITOR")]
		internal static void LogSignalDispatch(ASignal signal, string memberName, string sourceFilePath, int sourceLineNumber)
		{
			Profiler.BeginSample("LogDispatchSignal");
			OnSignalDispatch?.Invoke(signal, memberName, sourceFilePath, sourceLineNumber);
			Profiler.EndSample();
		}
	}

	public class SignalHub
	{
		private readonly Dictionary<Type, ABaseSignal> signals = new Dictionary<Type, ABaseSignal>();

		/// <summary>
		/// Getter for a signal of a given type
		/// </summary>
		/// <param name="signalType">Type of the Signal to retrieve</param>
		/// <returns>The proper signal binding</returns>
		public ABaseSignal Get(Type signalType)
		{
			ABaseSignal signal;

			if (signals.TryGetValue(signalType, out signal))
			{
				return signal;
			}

			return Bind(signalType);
		}
		
		/// <summary>
		/// Getter for a signal of a given type
		/// </summary>
		/// <typeparam name="T">Type of signal</typeparam>
		/// <returns>The proper signal binding</returns>
		public T Get<T>() where T : ABaseSignal
		{
			var signalType = typeof(T);
			return (T) Get(signalType);
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

		private ABaseSignal Bind(Type signalType)
		{
			ABaseSignal signal = (ABaseSignal) Activator.CreateInstance(signalType);
			signals.Add(signalType, signal);
			return signal;
		}
	}
}