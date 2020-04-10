// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThreeArgumentSignalTests.cs" company="Supyrb">
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
	public class ThreeArgumentSignalTests : ASignalTests<TestSignalObjectStringInt>
	{
		private const object SignalDispatchContext0 = null;
		private const string SignalDispatchContext1 = "Value";
		private const int SignalDispatchContext2 = 0;

		protected override void Dispatch()
		{
			signal.Dispatch(SignalDispatchContext0, SignalDispatchContext1, SignalDispatchContext2);
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

		private void OnListenerAWrapper(object c0, string c1, int c2)
		{
			OnListenerA();
		}

		private void OnListenerBWrapper(object c0, string c1, int c2)
		{
			OnListenerB();
		}

		private void OnListenerCWrapper(object c0, string c1, int c2)
		{
			OnListenerC();
		}

		private void OnPauseListenerWrapper(object c0, string c1, int c2)
		{
			OnPauseListener();
		}

		private void OnConsumeListenerWrapper(object c0, string c1, int c2)
		{
			OnConsumeListener();
		}

		private void OnUnsubscribeSelfListener(object c0, string c1, int c2)
		{
			signal.RemoveListener(OnUnsubscribeSelfListener);
		}


		private void OnAddListenerAAtMinusTen(object c0, string c1, int c2)
		{
			signal.AddListener(OnListenerAWrapper, -10);
		}

		private void OnAddListenerBAtZero(object c0, string c1, int c2)
		{
			signal.AddListener(OnListenerBWrapper, 0);
		}

		private void OnRemoveListenerA(object c0, string c1, int c2)
		{
			signal.RemoveListener(OnListenerAWrapper);
		}

		private void OnRemoveListenerB(object c0, string c1, int c2)
		{
			signal.RemoveListener(OnListenerBWrapper);
		}
	}
}