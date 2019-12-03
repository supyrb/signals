// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleSignal.cs" company="Supyrb">
//   Copyright (c) 2019 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------


namespace Supyrb
{
	public class BasicExampleSignal : Signal
	{
		// Override invoke to make sure that the derived signal class shows up in the stacktrace
		protected override void Invoke(int index)
		{
			base.Invoke(index);
		}
	}
}