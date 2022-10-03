// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializableSystemType.cs">
//   Copyright (c) 2022 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using UnityEngine;

namespace Supyrb
{
	[System.Serializable]
	public class SerializableSystemType : IComparable<SerializableSystemType>
	{
		[SerializeField] private string m_Name;

		public string Name
		{
			get { return m_Name; }
		}

		[SerializeField] private string m_AssemblyQualifiedName;

		public string AssemblyQualifiedName
		{
			get { return m_AssemblyQualifiedName; }
		}

		[SerializeField] private string m_AssemblyName;

		public string AssemblyName
		{
			get { return m_AssemblyName; }
		}

		private System.Type m_SystemType;

		public System.Type SystemType
		{
			get
			{
				if (m_SystemType == null)
				{
					GetSystemType();
				}

				return m_SystemType;
			}
		}

		private void GetSystemType()
		{
			m_SystemType = System.Type.GetType(m_AssemblyQualifiedName);
		}

		public SerializableSystemType(System.Type systemType)
		{
			m_SystemType = systemType;
			m_Name = systemType.Name;
			m_AssemblyQualifiedName = systemType.AssemblyQualifiedName;
			m_AssemblyName = systemType.Assembly.FullName;
		}

		public int CompareTo(SerializableSystemType other)
		{
			if (m_Name == null)
			{
				if (other == null || other.m_Name == null)
				{
					return 0;
				}

				return -1;
			}

			if (other == null || other.m_Name == null)
			{
				return 1;
			}

			return String.Compare(m_Name, other.m_Name, StringComparison.Ordinal);
		}
	}
}