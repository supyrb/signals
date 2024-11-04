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
    public struct CustomStruct
	{
		public int Integer;
		public string String;
	}

	public class CustomSerializedStructSignal : Signal<CustomStruct>
	{
	}
}