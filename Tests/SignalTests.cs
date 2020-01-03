// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalTests.cs" company="Supyrb">
//   Copyright (c) 2019 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using NUnit.Framework;

namespace Supyrb
{
	/// <summary>
	/// Tests the functionality of class Signal
	/// </summary>
	[TestFixture]
	[Category("Signals")]
	public class SignalTests
	{
		private TestSignal testSignal;
		private readonly List<string> callLog = new List<string>();

		[OneTimeSetUp]
		public void InitialSetup()
		{
			callLog.Clear();
			Signals.Clear();
			Signals.Get(out testSignal);
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
			testSignal.Clear();
		}

		[Test]
		public void ListenerOrderTest()
		{
			testSignal.AddListener(OnListenerA, -10);
			testSignal.AddListener(OnListenerB, 0);
			testSignal.AddListener(OnListenerC, 10);

			testSignal.Dispatch();

			Assert.IsTrue(callLog[0] == "A");
			Assert.IsTrue(callLog[1] == "B");
			Assert.IsTrue(callLog[2] == "C");
			Assert.IsTrue(testSignal.ListenerCount == 3);
		}
		
		[Test]
		public void RemoveListenerTest()
		{
			testSignal.AddListener(OnListenerA, -10);
			testSignal.AddListener(OnListenerB, 0);
			testSignal.AddListener(OnListenerC, 10);

			testSignal.RemoveListener(OnListenerA);
			testSignal.Dispatch();
			testSignal.RemoveListener(OnListenerB);
			testSignal.Dispatch();

			Assert.IsTrue(callLog[0] == "B");
			Assert.IsTrue(callLog[1] == "C");
			Assert.IsTrue(callLog[2] == "C");
			Assert.IsTrue(testSignal.ListenerCount == 1);
		}

		[Test]
		public void PauseContinueSignalTest()
		{
			testSignal.AddListener(OnListenerA, -10);
			testSignal.AddListener(OnPauseListener, 0);
			testSignal.AddListener(OnListenerB, 10);

			testSignal.Dispatch();

			Assert.IsTrue(callLog.Contains("A"));
			Assert.IsFalse(callLog.Contains("B"));
			
			ContinueSignal();
			
			Assert.IsTrue(callLog.Contains("B"));
		}
		
		[Test]
		public void InvalidContinueSignalTest()
		{
			testSignal.AddListener(OnListenerA, -10);
			testSignal.AddListener(OnListenerB, 0);

			ContinueSignal();
			
			Assert.IsFalse(callLog.Contains("A"));
			Assert.IsFalse(callLog.Contains("B"));
		}
		
		[Test]
		public void ConsumeSignalTest()
		{
			testSignal.AddListener(OnListenerA, -10);
			testSignal.AddListener(OnConsumeListener, 0);
			testSignal.AddListener(OnListenerB, 10);

			testSignal.Dispatch();

			Assert.IsTrue(callLog.Contains("A"));
			Assert.IsFalse(callLog.Contains("B"));
		}
		
		[Test]
		public void RemoveCurrentListenerWhileDispatching()
		{
			testSignal.AddListener(OnListenerA, -10);
			testSignal.AddListener(RemoveSelfListener, 0);
			testSignal.AddListener(OnListenerB, 10);

			testSignal.Dispatch();

			Assert.IsTrue(callLog.Contains("B"));
			Assert.IsTrue(testSignal.ListenerCount == 2);
		}
		
		[Test]
		public void RemoveLastListenerWhileDispatching()
		{
			testSignal.AddListener(OnListenerA, -10);
			testSignal.AddListener(RemoveListenerA, 0);
			testSignal.AddListener(OnListenerB, 10);

			testSignal.Dispatch();

			Assert.IsTrue(callLog.Contains("A"));
			Assert.IsTrue(callLog.Contains("B"));
			Assert.IsTrue(testSignal.ListenerCount == 2);
		}
		
		[Test]
		public void RemoveNextListenerWhileDispatching()
		{
			testSignal.AddListener(OnListenerA, -10);
			testSignal.AddListener(RemoveListenerB, 0);
			testSignal.AddListener(OnListenerB, 10);

			testSignal.Dispatch();

			Assert.IsTrue(callLog.Contains("A"));
			Assert.IsFalse(callLog.Contains("B"));
			Assert.IsTrue(testSignal.ListenerCount == 2);
		}
		
		[Test]
		public void RemoveNotExistingListener()
		{
			testSignal.RemoveListener(OnListenerA);
			testSignal.Dispatch();
			
			Assert.IsTrue(testSignal.ListenerCount == 0);
		}
		
		[Test]
		public void AddListenerTwice()
		{
			testSignal.AddListener(OnListenerA, -10);
			// Should be ignored
			testSignal.AddListener(OnListenerA, 0);
			testSignal.Dispatch();
			
			Assert.IsTrue(callLog.Contains("A"));
			Assert.IsTrue(callLog.Count == 1);
			Assert.IsTrue(testSignal.ListenerCount == 1);
		}
		
		[Test]
		public void AddSameOrderListenerWhileDispatching()
		{
			testSignal.AddListener(AddListenerAOrderMinusTen, -10);
			testSignal.AddListener(OnListenerB, 0);
			testSignal.Dispatch();
			
			Assert.IsFalse(callLog.Contains("A"));
			Assert.IsTrue(callLog.Count == 2);
			Assert.IsTrue(testSignal.ListenerCount == 3);
			
			testSignal.Dispatch();
			
			Assert.IsTrue(callLog.Contains("A"));
			Assert.IsTrue(callLog.Count == 5);
			Assert.IsTrue(testSignal.ListenerCount == 3);
		}
		
		[Test]
		public void AddLowerOrderListenerWhileDispatching()
		{
			testSignal.AddListener(AddListenerAOrderMinusTen, -5);
			testSignal.AddListener(OnListenerB, 0);
			testSignal.Dispatch();
			
			Assert.IsFalse(callLog.Contains("A"));
			Assert.IsTrue(callLog.Count == 2);
			Assert.IsTrue(testSignal.ListenerCount == 3);
			
			testSignal.Dispatch();
			
			Assert.IsTrue(callLog.Contains("A"));
			Assert.IsTrue(callLog.Count == 5);
			Assert.IsTrue(testSignal.ListenerCount == 3);
		}
		
		[Test]
		public void AddHigherOrderListenerWhileDispatching()
		{
			testSignal.AddListener(OnListenerA, -10);
			testSignal.AddListener(AddListenerBOrderTen, 0);
			testSignal.Dispatch();
			
			Assert.IsTrue(callLog.Contains("B"));
			Assert.IsTrue(callLog.Count == 3);
			Assert.IsTrue(testSignal.ListenerCount == 3);
		}
		
		private void OnListenerA()
		{
			callLog.Add("A");
		}

		private void OnListenerB()
		{
			callLog.Add("B");
		}
		
		private void OnListenerC()
		{
			callLog.Add("C");
		}
		
		private void RemoveSelfListener()
		{
			callLog.Add("Remove RemoveSelfListener");
			testSignal.RemoveListener(RemoveSelfListener);
		}
		
		private void RemoveListenerA()
		{
			callLog.Add("Remove ListenerA");
			testSignal.RemoveListener(OnListenerA);
		}
		
		private void AddListenerAOrderMinusTen()
		{
			callLog.Add("Add ListenerA");
			testSignal.AddListener(OnListenerA, -10);
		}
		
		private void RemoveListenerB()
		{
			callLog.Add("Remove ListenerB");
			testSignal.RemoveListener(OnListenerB);
		}
		
		private void AddListenerBOrderTen()
		{
			callLog.Add("Add ListenerB");
			testSignal.AddListener(OnListenerB, 10);
		}

		private void OnConsumeListener()
		{
			callLog.Add("Consume Signal");
			testSignal.Consume();
		}

		private void OnPauseListener()
		{
			callLog.Add("Pause Signal");
			testSignal.Pause();
		}

		private void ContinueSignal()
		{
			callLog.Add("Continue Signal");
			testSignal.Continue();
		}
	}
}