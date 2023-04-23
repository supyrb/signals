// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderedListTests.cs" company="Supyrb">
//   Copyright (c) 2023 Supyrb. All rights reserved.
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
	[Category("Signals")]
	public class OrderedListTests
	{
		[Test]
		public void OrderedListDefaultValueAdd()
		{
			var orderedList = new OrderedList<string>(false, 0);

			Assert.AreEqual(0, orderedList.Add(0, "firstDefault"));
			Assert.AreEqual(1, orderedList.Add(0, "secondDefault"));
			Assert.AreEqual(0, orderedList.Add(-10, "priorityItem"));
			Assert.AreEqual(3, orderedList.Add(0, "thirdDefault"));
			Assert.AreEqual(4, orderedList.Add(10, "lateItem"));
			Assert.AreEqual(4, orderedList.Add(0, "forthDefault"));
		}
	}
}