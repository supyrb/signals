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
	/// <summary>
	/// A log entry for a dispatched signal
	/// This class is fully compatible with Unity's serialization system and can therefore survive script reloads
	/// Can be used in the editor and in builds
	/// </summary>
	[Serializable]
	public class SignalLogEntry
	{
		[SerializeField]
		public SerializableDateTime TimeStamp;
		[SerializeField]
		public float PlayDispatchTime;
		[SerializeField]
		public SerializableSystemType SignalType;
		[SerializeField]
		public string MemberName;
		[SerializeField]
		public string SourceFilePath;
		[SerializeField]
		public string SourceFileName;
		[SerializeField]
		public int SourceLineNumber;

		public SignalLogEntry(ASignal signalInstance, string memberName, string sourceFilePath, int sourceLineNumber)
		{
			TimeStamp = new SerializableDateTime(DateTime.Now);
			PlayDispatchTime = Time.time;
			SignalType = new SerializableSystemType(signalInstance.GetType());
			MemberName = memberName;
			SourceFilePath = sourceFilePath;
			SourceFileName = Path.GetFileNameWithoutExtension(SourceFilePath);
			SourceLineNumber = sourceLineNumber;
		}
	}
}