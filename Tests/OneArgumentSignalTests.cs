// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OneArgumentSignalTests.cs" company="Supyrb">
//   Copyright (c) 2020 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using NUnit.Framework;

namespace Supyrb.Tests
{
	[TestFixture]
	public class OneArgumentSignalTests : ASignalTests<TestSignalInt>
	{
		private const int SignalDispatchValue = 0;

		protected override void Dispatch()
		{
			signal.Dispatch(SignalDispatchValue);
		}

		protected override void AddListenerA(int order = 0)
		{
			signal.AddListener(OnListenerAWrapper, order);
		}

		protected override void RemoveListenerA()
		{
			signal.RemoveListener(OnListenerAWrapper);
		}

		protected override void AddListenerB(int order = 0)
		{
			signal.AddListener(OnListenerBWrapper, order);
		}

		protected override void RemoveListenerB()
		{
			signal.RemoveListener(OnListenerBWrapper);
		}

		protected override void AddListenerC(int order = 0)
		{
			signal.AddListener(OnListenerCWrapper, order);
		}

		protected override void RemoveListenerC()
		{
			signal.RemoveListener(OnListenerCWrapper);
		}

		protected override void AddPauseListener(int order = 0)
		{
			signal.AddListener(OnPauseListenerWrapper, order);
		}

		protected override void AddConsumeListener(int order = 0)
		{
			signal.AddListener(OnConsumeListenerWrapper, order);
		}

		protected override void AddUnsubscribeSelfListener(int order = 0)
		{
			signal.AddListener(OnUnsubscribeSelfListener, order);
		}

		protected override void AddListenerAOnCallAtMinusTen(int order = 0)
		{
			signal.AddListener(OnAddListenerAAtMinusTen, order);
		}

		protected override void RemoveListenerAOnCall(int order = 0)
		{
			signal.AddListener(OnRemoveListenerA, order);
		}

		protected override void AddListenerBOnCallAtZero(int order = 0)
		{
			signal.AddListener(OnAddListenerBAtZero, order);
		}

		protected override void RemoveListenerBOnCall(int order = 0)
		{
			signal.AddListener(OnRemoveListenerB, order);
		}

		private void OnListenerAWrapper(int signalValue)
		{
			OnListenerA();
		}

		private void OnListenerBWrapper(int signalValue)
		{
			OnListenerB();
		}

		private void OnPauseListenerWrapper(int signalValue)
		{
			OnPauseListener();
		}

		private void OnConsumeListenerWrapper(int signalValue)
		{
			OnConsumeListener();
		}

		private void OnListenerCWrapper(int signalValue)
		{
			OnListenerC();
		}

		private void OnUnsubscribeSelfListener(int signalValue)
		{
			signal.RemoveListener(OnUnsubscribeSelfListener);
		}


		private void OnAddListenerAAtMinusTen(int signalValue)
		{
			signal.AddListener(OnListenerAWrapper, -10);
		}

		private void OnAddListenerBAtZero(int signalValue)
		{
			signal.AddListener(OnListenerBWrapper, 0);
		}

		private void OnRemoveListenerA(int signalValue)
		{
			signal.RemoveListener(OnListenerAWrapper);
		}

		private void OnRemoveListenerB(int signalValue)
		{
			signal.RemoveListener(OnListenerBWrapper);
		}
	}
}