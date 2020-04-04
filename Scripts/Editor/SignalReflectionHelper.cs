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
using UnityEngine;

namespace Supyrb
{
	public static class SignalReflectionHelper
	{
		public static void GetAllDerivedClasses<T>(ref List<Type> list) where T : ABaseSignal
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();

			for (int i = 0; i < assemblies.Length; i++)
			{
				var assembly = assemblies[i];
				if (!assembly.IsDynamic && !IsInProject(assembly.Location))
				{
					continue;
				}
				
				// Ignore Microsoft.CodeAnalysis
				// see https://issuetracker.unity3d.com/issues/reflectiontypeloadexception-is-thrown-when-retrieving-assembly-types-in-project-that-contains-immediate-window-package
				if (assembly.FullName.StartsWith(("Microsoft.CodeAnalysis")))
				{
					continue;
				}

				try
				{
					GetAllDerivedClasses<T>(ref list, assembly);
				}
				catch (ReflectionTypeLoadException e)
				{
					Debug.LogWarningFormat("Error when getting types of {0}, ignoring this assembly\n{1}", 
						assembly.FullName, e.Message);
				}
			}
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