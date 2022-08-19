// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Signal.cs" company="Supyrb">
//   Copyright (c) 2019 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;

namespace Supyrb
{
	public class Signal : ASignal
	{
		private readonly OrderedList<Action> listeners;

		/// <inheritdoc />
		public override int ListenerCount
		{
			get { return listeners.Count; }
		}

		public Signal() : base()
		{
			this.listeners = new OrderedList<Action>(true);
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
		public bool AddListener(Action listener, int order = 0)
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
		public bool RemoveListener(Action listener)
		{
			var index = listeners.Remove(listener);
			if (index < 0)
			{
				return false;
			}

			RemoveListenerAt(index);
			return true;
		}

		/// <summary>
		/// Dispatch the signal to the listeners in their defined order until the signal
		/// is consumed (<see cref="ASignal.Consume"/>) or paused (<see cref="ASignal.Pause"/>)
		/// </summary>
		public void Dispatch([CallerMemberName] string memberName = "",
							[CallerFilePath] string sourceFilePath = "",
							[CallerLineNumber] int sourceLineNumber = 0)
		{
			BeginSignalProfilerSample("Dispatch Signal");
			
			Signals.LogSignalDispatch(this, memberName, sourceFilePath, sourceLineNumber);
			StartDispatch();

			EndSignalProfilerSample();
		}

		protected override void Invoke(int index)
		{
			listeners[index].Invoke();
		}
	}

	public class Signal<T> : ASignal
	{
		private readonly OrderedList<Action<T>> listeners;
		private T context0;

		/// <inheritdoc />
		public override int ListenerCount
		{
			get { return listeners.Count; }
		}

		public Signal() : base()
		{
			this.listeners = new OrderedList<Action<T>>(true);
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
		public bool AddListener(Action<T> listener, int order = 0)
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
		public bool RemoveListener(Action<T> listener)
		{
			var index = listeners.Remove(listener);
			if (index < 0)
			{
				return false;
			}

			RemoveListenerAt(index);
			return true;
		}

		/// <summary>
		/// Dispatch the signal to the listeners in their defined order until the signal
		/// is consumed (<see cref="ASignal.Consume"/>) or paused (<see cref="ASignal.Pause"/>)
		/// </summary>
		public void Dispatch(T context0,
							[CallerMemberName] string memberName = "",
							[CallerFilePath] string sourceFilePath = "",
							[CallerLineNumber] int sourceLineNumber = 0)
		{
			BeginSignalProfilerSample("Dispatch Signal");
			
			this.context0 = context0;
			Signals.LogSignalDispatch(this, memberName, sourceFilePath, sourceLineNumber);
			StartDispatch();

			EndSignalProfilerSample();
		}

		protected override void Invoke(int index)
		{
			listeners[index].Invoke(context0);
		}

		protected override void OnFinish()
		{
			base.OnFinish();
			context0 = default(T);
		}
	}

	public class Signal<T, U> : ASignal
	{
		private readonly OrderedList<Action<T, U>> listeners;
		private T context0;
		private U context1;

		/// <inheritdoc />
		public override int ListenerCount
		{
			get { return listeners.Count; }
		}

		public Signal() : base()
		{
			this.listeners = new OrderedList<Action<T, U>>(true);
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
		public bool AddListener(Action<T, U> listener, int order = 0)
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
		public bool RemoveListener(Action<T, U> listener)
		{
			var index = listeners.Remove(listener);
			if (index < 0)
			{
				return false;
			}

			RemoveListenerAt(index);
			return true;
		}

		/// <summary>
		/// Dispatch the signal to the listeners in their defined order until the signal
		/// is consumed (<see cref="ASignal.Consume"/>) or paused (<see cref="ASignal.Pause"/>)
		/// </summary>
		public void Dispatch(T context0, U context1,
							[CallerMemberName] string memberName = "",
							[CallerFilePath] string sourceFilePath = "",
							[CallerLineNumber] int sourceLineNumber = 0)
		{
			BeginSignalProfilerSample("Dispatch Signal");
			
			this.context0 = context0;
			this.context1 = context1;
			Signals.LogSignalDispatch(this, memberName, sourceFilePath, sourceLineNumber);
			StartDispatch();

			EndSignalProfilerSample();
		}

		protected override void Invoke(int index)
		{
			listeners[index].Invoke(context0, context1);
		}

		protected override void OnFinish()
		{
			base.OnFinish();
			context0 = default(T);
			context1 = default(U);
		}
	}

	public class Signal<T, U, V> : ASignal
	{
		private readonly OrderedList<Action<T, U, V>> listeners;
		private T context0;
		private U context1;
		private V context2;

		/// <inheritdoc />
		public override int ListenerCount
		{
			get { return listeners.Count; }
		}

		public Signal() : base()
		{
			this.listeners = new OrderedList<Action<T, U, V>>(true);
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
		public bool AddListener(Action<T, U, V> listener, int order = 0)
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
		public bool RemoveListener(Action<T, U, V> listener)
		{
			var index = listeners.Remove(listener);
			if (index < 0)
			{
				return false;
			}

			RemoveListenerAt(index);
			return true;
		}

		/// <summary>
		/// Dispatch the signal to the listeners in their defined order until the signal
		/// is consumed (<see cref="ASignal.Consume"/>) or paused (<see cref="ASignal.Pause"/>)
		/// </summary>
		public void Dispatch(T context0, U context1, V context2,
							[CallerMemberName] string memberName = "",
							[CallerFilePath] string sourceFilePath = "",
							[CallerLineNumber] int sourceLineNumber = 0)
		{
			BeginSignalProfilerSample("Dispatch Signal");
			
			this.context0 = context0;
			this.context1 = context1;
			this.context2 = context2;
			Signals.LogSignalDispatch(this, memberName, sourceFilePath, sourceLineNumber);
			StartDispatch();

			EndSignalProfilerSample();
		}

		protected override void Invoke(int index)
		{
			listeners[index].Invoke(context0, context1, context2);
		}

		protected override void OnFinish()
		{
			base.OnFinish();
			context0 = default(T);
			context1 = default(U);
			context2 = default(V);
		}
	}
}