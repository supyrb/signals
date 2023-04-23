// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalTests.cs" company="Supyrb">
//   Copyright (c) 2019 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Supyrb.Tests
{
	/// <summary>
	/// Tests the functionality of class Signal
	/// </summary>
	[TestFixture]
	[Category("Signals")]
	public abstract class ASignalTests<T> where T: ASignal
	{
		protected T signal;
		private readonly List<string> callLog = new List<string>();

		[OneTimeSetUp]
		public void InitialSetup()
		{
			callLog.Clear();
			Signals.Clear();
			Signals.Get(out signal);
		}

		[OneTimeTearDown]
		public void FinalTearDown()
		{
			Signals.Clear();
		}

		[TearDown]
		public void TearDown()
		{
			callLog.Clear();
			signal.Clear();
		}

		[Test]
		public void ListenerOrderTest()
		{
			AddListenerA(-10);
			AddListenerB(0);
			AddListenerC(10);

			Dispatch();

			Assert.AreEqual("A,B,C", string.Join(",", callLog));
			Assert.AreEqual(3, signal.ListenerCount);
		}

		[Test]
		public void RemoveListenerTest()
		{
			AddListenerA(-10);
			AddListenerB(0);
			AddListenerC(10);

			RemoveListenerA();
			Dispatch();
			RemoveListenerB();
			Dispatch();

			Assert.AreEqual("B,C,C", string.Join(",", callLog));
			Assert.AreEqual(1, signal.ListenerCount);
		}

		[Test]
		public void PauseContinueSignalTest()
		{
			AddListenerA(-10);
			AddPauseListener(0);
			AddListenerB(10);

			Dispatch();

			Assert.IsTrue(callLog.Contains("A"));
			Assert.IsFalse(callLog.Contains("B"));

			ContinueSignal();

			Assert.AreEqual("A,Pause,Continue,B", string.Join(",", callLog));
		}

		[Test]
		public void InvalidContinueSignalTest()
		{
			AddListenerA(-10);
			AddListenerB(0);

			ContinueSignal();

			Assert.IsFalse(callLog.Contains("A"));
			Assert.IsFalse(callLog.Contains("B"));
		}

		[Test]
		public void ConsumeSignalTest()
		{
			AddListenerA(-10);
			AddConsumeListener(0);
			AddListenerB(10);

			Dispatch();

			Assert.IsTrue(callLog.Contains("A"));
			Assert.IsFalse(callLog.Contains("B"));
		}

		[Test]
		public void RemoveCurrentListenerWhileDispatching()
		{
			AddListenerA(-10);
			AddUnsubscribeSelfListener(0);
			AddListenerB(10);

			Dispatch();

			Assert.AreEqual("A,B", string.Join(",", callLog));
			Assert.AreEqual(2, signal.ListenerCount);
		}

		[Test]
		public void RemovePreviousListenerWhileDispatching()
		{
			AddListenerA(-10);
			RemoveListenerAOnCall(0);
			AddListenerB(10);

			Dispatch();

			Assert.AreEqual("A,B", string.Join(",", callLog));
			Assert.AreEqual(2, signal.ListenerCount);
		}

		[Test]
		public void RemoveNextListenerWhileDispatching()
		{
			AddListenerA(-10);
			RemoveListenerBOnCall(0);
			AddListenerB(10);

			Dispatch();

			Assert.IsTrue(callLog.Contains("A"));
			Assert.IsFalse(callLog.Contains("B"));
			Assert.AreEqual(2, signal.ListenerCount);
		}

		[Test]
		public void RemoveNotExistingListener()
		{
			RemoveListenerA();
			Dispatch();

			Assert.AreEqual(0, signal.ListenerCount);
		}

		[Test]
		public void AddListenerTwice()
		{
			AddListenerA(-10);
			// Should be ignored
			AddListenerA(0);
			Dispatch();

			Assert.AreEqual("A", string.Join(",", callLog));
			Assert.AreEqual(1, signal.ListenerCount);
		}

		/// <summary>
		/// Listener with same priority should be added at the end of the list of that priority
		/// Therefore it should be called when added at the same priority right away
		/// </summary>
		[Test]
		public void AddSameOrderListenerWhileDispatching()
		{
			AddListenerAOnCallAtMinusTen(-10);
			AddListenerB(0);
			Dispatch();

			Assert.AreEqual("A,B", string.Join(",", callLog));
			Assert.AreEqual(3, signal.ListenerCount);

			Dispatch();

			Assert.AreEqual("A,B,A,B", string.Join(",", callLog));
			Assert.AreEqual(3, signal.ListenerCount);
		}

		[Test]
		public void AddLowerOrderListenerWhileDispatching()
		{
			AddListenerAOnCallAtMinusTen(-5);
			AddListenerB(0);
			Dispatch();

			Assert.AreEqual("B", string.Join(",", callLog));
			Assert.AreEqual(3, signal.ListenerCount);

			Dispatch();

			Assert.AreEqual("B,A,B", string.Join(",", callLog));
			Assert.AreEqual(3, signal.ListenerCount);
		}

		[Test]
		public void AddHigherOrderListenerWhileDispatching()
		{
			AddListenerA(-10);
			AddListenerBOnCallAtZero(-5);
			Dispatch();

			Assert.AreEqual("A,B", string.Join(",", callLog));
			Assert.IsTrue(signal.ListenerCount == 3);
		}

		protected abstract void Dispatch();
		protected abstract void AddListenerA(int order = 0);
		protected abstract void RemoveListenerA();
		protected abstract void AddListenerB(int order = 0);
		protected abstract void RemoveListenerB();
		protected abstract void AddListenerC(int order = 0);
		protected abstract void RemoveListenerC();
		protected abstract void AddPauseListener(int order = 0);
		protected abstract void AddConsumeListener(int order = 0);

		protected abstract void AddUnsubscribeSelfListener(int order = 0);

		protected abstract void AddListenerAOnCallAtMinusTen(int order = 0);
		protected abstract void RemoveListenerAOnCall(int order = 0);

		protected abstract void AddListenerBOnCallAtZero(int order = 0);
		protected abstract void RemoveListenerBOnCall(int order = 0);

		protected void OnListenerA()
		{
			callLog.Add("A");
		}

		protected void OnListenerB()
		{
			callLog.Add("B");
		}

		protected void OnListenerC()
		{
			callLog.Add("C");
		}

		protected void OnConsumeListener()
		{
			callLog.Add("Consume");
			signal.Consume();
		}

		protected void OnPauseListener()
		{
			callLog.Add("Pause");
			signal.Pause();
		}

		private void ContinueSignal()
		{
			callLog.Add("Continue");
			signal.Continue();
		}
	}
}