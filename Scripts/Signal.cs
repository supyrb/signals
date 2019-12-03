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
using UnityEngine.Profiling;

namespace Supyrb
{
	public class Signal : ABaseSignal
	{
		private readonly OrderedList<Action> listeners;

		public override int ListenerCount => listeners.Count;

		public Signal() : base()
		{
			this.listeners = new OrderedList<Action>(true);
		}

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

		public void Dispatch()
		{
			Profiler.BeginSample("Dispatch Signal");
			{
				Profiler.BeginSample(this.GetType().FullName);
				{
					CleanupForDispatch();
					Run();
				}
				Profiler.EndSample();
			}
			Profiler.EndSample();
		}

		protected override void Invoke(int index)
		{
			listeners[currentIndex].Invoke();
		}
	}

	public class Signal<T> : ABaseSignal
	{
		private readonly OrderedList<Action<T>> listeners;
		private T context;

		public override int ListenerCount => listeners.Count;

		public Signal() : base()
		{
			this.listeners = new OrderedList<Action<T>>(true);
		}

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

		public void Dispatch(T context)
		{
			Profiler.BeginSample("Dispatch Signal");
			{
				Profiler.BeginSample(this.GetType().FullName);
				{
					CleanupForDispatch();
					this.context = context;
					Run();
				}
				Profiler.EndSample();
			}
			Profiler.EndSample();
		}

		protected override void Invoke(int index)
		{
			listeners[currentIndex].Invoke(context);
		}

		protected override void OnFinish()
		{
			base.OnFinish();
			context = default(T);
		}
	}

	public class Signal<T, U> : ABaseSignal
	{
		private readonly OrderedList<Action<T, U>> listeners;
		private T context0;
		private U context1;

		public override int ListenerCount => listeners.Count;

		public Signal() : base()
		{
			this.listeners = new OrderedList<Action<T, U>>(true);
		}

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

		public void Dispatch(T context0, U context1)
		{
			Profiler.BeginSample("Dispatch Signal");
			{
				Profiler.BeginSample(this.GetType().FullName);
				{
					CleanupForDispatch();
					this.context0 = context0;
					this.context1 = context1;
					Run();
				}
				Profiler.EndSample();
			}
			Profiler.EndSample();
		}

		protected override void Invoke(int index)
		{
			listeners[currentIndex].Invoke(context0, context1);
		}

		protected override void OnFinish()
		{
			base.OnFinish();
			context0 = default(T);
			context1 = default(U);
		}
	}

	public class Signal<T, U, V> : ABaseSignal
	{
		private readonly OrderedList<Action<T, U, V>> listeners;
		private T context0;
		private U context1;
		private V context2;

		public override int ListenerCount => listeners.Count;

		public Signal() : base()
		{
			this.listeners = new OrderedList<Action<T, U, V>>(true);
		}

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

		public void Dispatch(T context0, U context1, V context2)
		{
			Profiler.BeginSample("Dispatch Signal");
			{
				Profiler.BeginSample(this.GetType().FullName);
				{
					CleanupForDispatch();
					this.context0 = context0;
					this.context1 = context1;
					this.context2 = context2;
					Run();
				}
				Profiler.EndSample();
			}
			Profiler.EndSample();
		}

		protected override void Invoke(int index)
		{
			listeners[currentIndex].Invoke(context0, context1, context2);
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