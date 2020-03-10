// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalReflectionHelper.cs" company="Supyrb">
//   Copyright (c) 2020 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Supyrb
{
	public static class SignalReflectionHelper
	{
		private static readonly string[] AssemblyBlackList = new[] {"Microsoft.CodeAnalysis.*"};
		
		public static void GetAllDerivedClasses<T>(ref List<Type> list) where T : ABaseSignal
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();

			for (int i = 0; i < assemblies.Length; i++)
			{
				var assembly = assemblies[i];
				if (!IsInProject(assembly.Location))
				{
					continue;
				}

				if (IsAssemblyBlackListed(assembly.FullName))
				{
					continue;
				}

				GetAllDerivedClasses<T>(ref list, assembly);
			}
		}

		private static bool IsAssemblyBlackListed(string assemblyFullName)
		{
			for (int i = 0; i < AssemblyBlackList.Length; i++)
			{
				var blackListRegex = AssemblyBlackList[i];
				if (Regex.IsMatch(assemblyFullName, blackListRegex))
				{
					return true;
				}
			}

			return false;
		}

		private static bool IsInProject(string path)
		{
			var assetsPath = Application.dataPath;
			var projectPath = assetsPath.Substring(0, assetsPath.Length - "/Assets".Length);
			path = path.Replace('\\', '/');
			return path.StartsWith(projectPath);
		}

		public static void GetAllDerivedClasses<T>(ref List<Type> list, Assembly assembly) where T : ABaseSignal
		{
			var types = assembly.GetTypes();
			var baseType = typeof(T);

			for (int i = 0; i < types.Length; i++)
			{
				var type = types[i];
				if (!type.IsClass || type.IsAbstract || !type.IsSubclassOf(baseType) || type.ContainsGenericParameters)
				{
					continue;
				}

				list.Add(type);
			}
		}
	}
}