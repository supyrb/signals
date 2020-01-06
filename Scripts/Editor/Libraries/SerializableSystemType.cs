// Simple helper class that allows you to serialize System.Type objects.
// Use it however you like, but crediting or even just contacting the author would be appreciated (Always 
// nice to see people using your stuff!)
//
// Written by Bryan Keiren (http://www.bryankeiren.com)

using UnityEngine;
using System.Runtime.Serialization;

namespace Supyrb
{
	[System.Serializable]
	public class SerializableSystemType
	{
		[SerializeField]
		private string m_Name;
	
		public string Name
		{
			get { return m_Name; }
		}
	
		[SerializeField]
		private string m_AssemblyQualifiedName;
	
		public string AssemblyQualifiedName
		{
			get { return m_AssemblyQualifiedName; }
		}
	
		[SerializeField]
		private string m_AssemblyName;
	
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
	
		public SerializableSystemType( System.Type systemType )
		{
			m_SystemType = systemType;
			m_Name = systemType.Name;
			m_AssemblyQualifiedName = systemType.AssemblyQualifiedName;
			m_AssemblyName = systemType.Assembly.FullName;
		}
	}

}
