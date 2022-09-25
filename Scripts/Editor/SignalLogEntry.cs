// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalLogEntry.cs">
//   Copyright (c) 2022 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using UnityEngine;

namespace Supyrb
{
	public class SignalLogEntry
	{
		public readonly DateTime TimeStamp;
		public readonly float PlayDispatchTime;
		public readonly ASignal SignalInstance;
		public readonly Type SignalType;
		public readonly string MemberName;
		public readonly string SourceFilePath;
		public readonly string SourceFileName;
		public readonly int SourceLineNumber;

		public SignalLogEntry(ASignal signalInstance, string memberName, string sourceFilePath, int sourceLineNumber)
		{
			TimeStamp = DateTime.Now;
			PlayDispatchTime = Time.time;
			SignalInstance = signalInstance;
			SignalType = signalInstance.GetType();
			MemberName = memberName;
			SourceFilePath = sourceFilePath;
			SourceFileName = Path.GetFileName(SourceFilePath);
			SourceLineNumber = sourceLineNumber;
		}
	}
}