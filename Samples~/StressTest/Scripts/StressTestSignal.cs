// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StressTestSignal.cs" company="Supyrb">
//   Copyright (c) 2019 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

namespace Supyrb
{
	public class StressTestSignal : Signal
	{
		// Override invoke to make sure that the derived signal class shows up in the stacktrace
		protected override void Invoke(int index)
		{
			base.Invoke(index);
		}
	}
}