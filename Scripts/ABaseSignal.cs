// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ABaseSignal.cs" company="Supyrb">
//   Copyright (c) 2019 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine.Profiling;

namespace Supyrb
{
	/// <summary>
	/// Base interface for Signals
	/// </summary>
	public interface ISignal
	{
		/// <summary>
		/// Unique id for this signal
		/// </summary>
		string Hash { get; }
	}

	public abstract class ABaseSignal : ISignal
	{
		protected int currentIndex;
		private bool consumed;
		private bool paused;
		private bool finished;

		protected string _hash;


		/// <inheritdoc />
		public string Hash
		{
			get
			{
				if (string.IsNullOrEmpty(_hash))
				{
					_hash = this.GetType().ToString();
				}

				return _hash;
			}
		}

		/// <summary>
		/// Number of registered listeners
		/// </summary>
		public abstract int ListenerCount { get; }
		

		protected ABaseSignal()
		{
			this.currentIndex = 0;
			this.consumed = false;
			this.paused = false;
			this.finished = true;
		}
		
		/// <summary>
		/// Removes all registered listeners
		/// </summary>
		public abstract void Clear();

		/// <summary>
		/// Pause dispatching
		/// Dispatching can be continued by calling <see cref="Continue"/> 
		/// </summary>
		public void Pause()
		{
			paused = true;
		}

		/// <summary>
		/// Continue dispatching
		/// Only applicable if <see cref="Pause"/> was called before
		/// </summary>
		public void Continue()
		{
			if (!paused)
			{
				return;
			}

			BeginContinueProfilerSample();
			
			paused = false;
			Run();
			
			EndContinueProfilerSample();
		}

		/// <summary>
		/// Consume the signal, no further listener will receive the dispatched signal
		/// </summary>
		public void Consume()
		{
			consumed = true;
		}
		
		protected void CleanupForDispatch()
		{
			currentIndex = 0;
			consumed = false;
			paused = false;
			finished = false;
		}

		protected void Run()
		{
			while (true)
			{
				if (paused || finished || consumed)
				{
					return;
				}

				if (currentIndex >= ListenerCount)
				{
					OnFinish();
					return;
				}

				Invoke(currentIndex);
				currentIndex++;
			}
		}

		protected void AddListenerAt(int index)
		{
			if (finished || consumed)
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
			if (finished || consumed)
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
			finished = true;
		}

		protected void BeginDispatchProfilerSample()
		{
			Profiler.BeginSample("Dispatch Signal");
			BeginSignalNameProfilerSample();
		}
		
		protected void EndDispatchProfilerSample()
		{
			Profiler.EndSample();
			EndSignalNameProfilerSample();
		}
		
		protected void BeginContinueProfilerSample()
		{
			Profiler.BeginSample("Continue Signal");
			BeginSignalNameProfilerSample();
		}
		
		protected void EndContinueProfilerSample()
		{
			Profiler.EndSample();
			EndSignalNameProfilerSample();
		}
		
		protected void BeginSignalNameProfilerSample()
		{
			Profiler.BeginSample(this.GetType().FullName);
		}
		
		protected void EndSignalNameProfilerSample()
		{
			Profiler.EndSample();
		}

		protected abstract void Invoke(int index);

		/// <inheritdoc />
		public override string ToString()
		{
			string state;
			if (paused)
			{
				state = "Paused at " + currentIndex;
			}
			else if (consumed)
			{
				state = "Consumed at " + (currentIndex - 1);
			}
			else if (currentIndex > 0 && !finished)
			{
				state = "Running at " + currentIndex;
			}
			else
			{
				state = "Idle";
			}

			return string.Format("Signal {0}: {1} Listeners, Current state {2}", this.GetType().Name, ListenerCount, state);
		}
	}
}