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
	public abstract class ASignal : ABaseSignal
	{
		protected int currentIndex;
		private bool consumed;
		private bool paused;
		private bool finished;


		/// <summary>
		/// Number of registered listeners
		/// </summary>
		public abstract int ListenerCount { get; }
		

		protected ASignal() : base()
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

			BeginSignalProfilerSample("Continue Signal");
			
			paused = false;
			Run();
			
			EndSignalProfilerSample();
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

		protected void BeginSignalProfilerSample(string sampleName)
		{
			Profiler.BeginSample(sampleName);
			Profiler.BeginSample(this.GetType().FullName);
		}
		
		protected void EndSignalProfilerSample()
		{
			Profiler.EndSample();
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