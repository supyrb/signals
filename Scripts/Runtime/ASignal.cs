// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ABaseSignal.cs" company="Supyrb">
//   Copyright (c) 2019 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

#if ENABLE_MONO || ENABLE_IL2CPP
using UnityEngine.Profiling;
using UnityEngine.Scripting;
#endif

namespace Supyrb
{
	/// <summary>
	/// The base implementation for a signal which defines all interfaces the signal should have
	/// This is also a non-generics version that can be easily referenced.
	/// Implementations with generics should take <see cref="ASignalAction"/> as their base class
	/// </summary>
#if ENABLE_MONO || ENABLE_IL2CPP
	[RequireDerived]
#endif
	public abstract class ASignal : ISignal
	{
		public enum State
		{
			/// <summary>
			/// Signal was never called or is finished
			/// </summary>
			Idle,
			/// <summary>
			/// Signal is currently active and processing listener at <see cref="ASignal.currentIndex"/>
			/// </summary>
			Running,
			/// <summary>
			/// Signal is paused and can be continued at the next index by calling <see cref="ASignal.Continue"/>
			/// </summary>
			Paused,
			/// <summary>
			/// Signal was consumed at <see cref="ASignal.currentIndex"/>
			/// </summary>
			Consumed
		}

		/// <summary>
		/// Number of registered listeners
		/// </summary>
		public abstract int ListenerCount { get; }

		private int currentIndex;
		private State state;


		protected ASignal()
		{
			this.currentIndex = 0;
			this.state = State.Idle;
		}

		/// <summary>
		/// Pause dispatching
		/// Dispatching can be continued by calling <see cref="Continue"/>
		/// </summary>
		public void Pause()
		{
			this.state = State.Paused;
		}

		/// <summary>
		/// Continue dispatching
		/// Only applicable if <see cref="Pause"/> was called before
		/// </summary>
		public void Continue()
		{
			if (state != State.Paused)
			{
				return;
			}

			BeginSignalProfilerSample("Continue Signal");

			currentIndex++;
			state = State.Running;
			Run();

			EndSignalProfilerSample();
		}

		/// <summary>
		/// Consume the signal, no further listener will receive the dispatched signal
		/// </summary>
		public void Consume()
		{
			state = State.Consumed;
		}

		protected void StartDispatch()
		{
			currentIndex = 0;
			state = State.Running;

			Run();
		}

		private void Run()
		{
			while (true)
			{
				if (currentIndex >= ListenerCount)
				{
					OnFinish();
					return;
				}

				Invoke(currentIndex);

				if (state != State.Running)
				{
					return;
				}
				currentIndex++;
			}
		}

		protected void AddListenerAt(int index)
		{
			if (state == State.Idle)
			{
				return;
			}

			if (currentIndex >= index)
			{
				currentIndex++;
			}
		}

		protected void RemoveListenerAt(int index)
		{
			if (state == State.Idle)
			{
				return;
			}

			if (currentIndex >= index)
			{
				currentIndex--;
			}
		}

		protected virtual void OnFinish()
		{
			state = State.Idle;
		}

		protected void BeginSignalProfilerSample(string sampleName)
		{
			#if ENABLE_MONO || ENABLE_IL2CPP
			Profiler.BeginSample(sampleName);
			Profiler.BeginSample(this.GetType().FullName);
			#endif
		}

		protected void EndSignalProfilerSample()
		{
			#if ENABLE_MONO || ENABLE_IL2CPP
			Profiler.EndSample();
			Profiler.EndSample();
			#endif
		}

				/// <inheritdoc />
		public override string ToString()
		{

			return string.Format("Signal {0}: {1} Listeners, State {2}, Index {3}",
				this.GetType().Name, ListenerCount, state, currentIndex);
		}

		/// <summary>
		/// Removes all registered listeners
		/// </summary>
		public abstract void Clear();

		protected abstract void Invoke(int index);
	}
}