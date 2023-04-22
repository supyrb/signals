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
	public class Signal : ASignalAction<Action>
	{

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

	public class Signal<T> : ASignalAction<Action<T>>
	{
		private T context0;

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

	public class Signal<T, U> : ASignalAction<Action<T, U>>
	{
		private T context0;
		private U context1;

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

	public class Signal<T, U, V> : ASignalAction<Action<T, U, V>>
	{
		private T context0;
		private U context1;
		private V context2;

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