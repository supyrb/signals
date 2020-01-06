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

namespace Supyrb
{
	public static class SignalReflectionHelper
	{
		public static void GetAllDerivedClasses<T>(ref List<Type> list) where T: ABaseSignal
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();

			for (int i = 0; i < assemblies.Length; i++)
			{
				var assembly = assemblies[i];
				GetAllDerivedClasses<T>(ref list, assembly);
			}
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