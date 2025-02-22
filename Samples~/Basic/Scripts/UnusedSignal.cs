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
	/// <summary>
	/// Signal that is not referenced by any other code.
	/// Shows how [RequireDerived] can be used to prevent the signal from being stripped by the linker.
	/// </summary>
	public class UnusedSignal : Signal
	{
	}
}