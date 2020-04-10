// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TwoArgumentSignalTests.cs" company="Supyrb">
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
	public class TwoArgumentSignalTests : ASignalTests<TestSignalStringInt>
	{
		private const string SignalDispatchContext0 = "Value";
		private const int SignalDispatchContext1 = 0;

		protected override void Dispatch()
		{
			signal.Dispatch(SignalDispatchContext0, SignalDispatchContext1);
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

		private void OnListenerAWrapper(string c0, int c1)
		{
			OnListenerA();
		}

		private void OnListenerBWrapper(string c0, int c1)
		{
			OnListenerB();
		}

		private void OnPauseListenerWrapper(string c0, int c1)
		{
			OnPauseListener();
		}

		private void OnConsumeListenerWrapper(string c0, int c1)
		{
			OnConsumeListener();
		}

		private void OnListenerCWrapper(string c0, int c1)
		{
			OnListenerC();
		}

		private void OnUnsubscribeSelfListener(string c0, int c1)
		{
			signal.RemoveListener(OnUnsubscribeSelfListener);
		}


		private void OnAddListenerAAtMinusTen(string c0, int c1)
		{
			signal.AddListener(OnListenerAWrapper, -10);
		}

		private void OnAddListenerBAtZero(string c0, int c1)
		{
			signal.AddListener(OnListenerBWrapper, 0);
		}

		private void OnRemoveListenerA(string c0, int c1)
		{
			signal.RemoveListener(OnListenerAWrapper);
		}

		private void OnRemoveListenerB(string c0, int c1)
		{
			signal.RemoveListener(OnListenerBWrapper);
		}
	}
}