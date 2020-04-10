// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalRegistryTests.cs" company="Supyrb">
//   Copyright (c) 2019 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using NUnit.Framework;

namespace Supyrb.Tests
{
	/// <summary>
	/// Tests the functionality of class Signals
	/// </summary>
	[TestFixture]
	[Category("Signal Registry")]
	public class SignalRegistryTests
	{
		[SetUp]
		[OneTimeTearDown]
		public void Setup()
		{
			Signals.Clear();
		}

		[Test]
		public void GetSignalFromRegistryTest()
		{
			TestSignal testSignal;
			Signals.Get(out testSignal);
			Assert.NotNull(testSignal);
		}

		[Test]
		public void SignalRegistryClearTest()
		{
			Signals.Get<TestSignal>();
			Assert.IsTrue(Signals.Count == 1);
			Signals.Clear();
			Assert.IsTrue(Signals.Count == 0);
		}

		[Test]
		public void SignalRegistryCountTest()
		{
			Signals.Clear();
			Signals.Get<TestSignal>();
			Assert.IsTrue(Signals.Count == 1);
			Signals.Get<TestSignal>();
			Assert.IsTrue(Signals.Count == 1);
			Signals.Get<TestSignalInt>();
			Assert.IsTrue(Signals.Count == 2);
		}

		[Test]
		public void SignalRegistryGetMethodsTest()
		{
			Signals.Clear();
			TestSignal testSignal0;
			TestSignal testSignal1;
			TestSignal testSignal2;

			testSignal0 = Signals.Get<TestSignal>();
			Signals.Get(out testSignal1);
			testSignal2 = (TestSignal) Signals.Get(typeof(TestSignal));

			Assert.IsTrue(testSignal0 == testSignal1);
			Assert.IsTrue(testSignal0 == testSignal2);
		}
	}
}