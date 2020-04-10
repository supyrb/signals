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

			Assert.IsTrue(callLog[0] == "A");
			Assert.IsTrue(callLog[1] == "B");
			Assert.IsTrue(callLog[2] == "C");
			Assert.IsTrue(signal.ListenerCount == 3);
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

			Assert.IsTrue(callLog[0] == "B");
			Assert.IsTrue(callLog[1] == "C");
			Assert.IsTrue(callLog[2] == "C");
			Assert.IsTrue(signal.ListenerCount == 1);
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
			
			Assert.IsTrue(callLog.Contains("B"));
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

			Assert.IsTrue(callLog.Contains("B"));
			Assert.IsTrue(signal.ListenerCount == 2);
		}
		
		[Test]
		public void RemoveLastListenerWhileDispatching()
		{
			AddListenerA(-10);
			RemoveListenerAOnCall(0);
			AddListenerB(10);

			Dispatch();

			Assert.IsTrue(callLog.Contains("A"));
			Assert.IsTrue(callLog.Contains("B"));
			Assert.IsTrue(signal.ListenerCount == 2);
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
			Assert.IsTrue(signal.ListenerCount == 2);
		}
		
		[Test]
		public void RemoveNotExistingListener()
		{
			RemoveListenerA();
			Dispatch();
			
			Assert.IsTrue(signal.ListenerCount == 0);
		}
		
		[Test]
		public void AddListenerTwice()
		{
			AddListenerA(-10);
			// Should be ignored
			AddListenerA(0);
			Dispatch();
			
			Assert.IsTrue(callLog.Contains("A"));
			Assert.IsTrue(callLog.Count == 1);
			Assert.IsTrue(signal.ListenerCount == 1);
		}
		
		[Test]
		public void AddSameOrderListenerWhileDispatching()
		{
			AddListenerAOnCallAtMinusTen(-10);
			AddListenerB(0);
			Dispatch();
			
			Assert.IsFalse(callLog.Contains("A"));
			Assert.IsTrue(callLog.Count == 1);
			Assert.IsTrue(signal.ListenerCount == 3);
			
			Dispatch();
			
			Assert.IsTrue(callLog.Contains("A"));
			Assert.IsTrue(callLog.Count == 3);
			Assert.IsTrue(signal.ListenerCount == 3);
		}
		
		[Test]
		public void AddLowerOrderListenerWhileDispatching()
		{
			AddListenerAOnCallAtMinusTen(-5);
			AddListenerB(0);
			Dispatch();
			
			Assert.IsFalse(callLog.Contains("A"));
			Assert.IsTrue(callLog.Count == 1);
			Assert.IsTrue(signal.ListenerCount == 3);
			
			Dispatch();
			
			Assert.IsTrue(callLog.Contains("A"));
			Assert.IsTrue(callLog.Count == 3);
			Assert.IsTrue(signal.ListenerCount == 3);
		}
		
		[Test]
		public void AddHigherOrderListenerWhileDispatching()
		{
			AddListenerA(-10);
			AddListenerBOnCallAtZero(-5);
			Dispatch();
			
			Assert.IsTrue(callLog.Contains("B"));
			Assert.IsTrue(callLog.Count == 2);
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
			callLog.Add("Consume Signal");
			signal.Consume();
		}

		protected void OnPauseListener()
		{
			callLog.Add("Pause Signal");
			signal.Pause();
		}

		private void ContinueSignal()
		{
			callLog.Add("Continue Signal");
			signal.Continue();
		}
	}
}