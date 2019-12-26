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
		public void RemoveListenerWhileDispatching()
		{
			testSignal.AddListener(OnListenerA, -10);
			testSignal.AddListener(OnRemoveSelfListener, 0);
			testSignal.AddListener(OnListenerB, 10);

			testSignal.Dispatch();

			Assert.IsTrue(callLog.Contains("B"));
			Assert.IsTrue(testSignal.ListenerCount == 2);
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
		
		private void OnRemoveSelfListener()
		{
			callLog.Add("Remove SemoveSelfListener");
			testSignal.RemoveListener(OnRemoveSelfListener);
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