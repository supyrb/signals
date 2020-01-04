// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ABaseSignal.cs" company="Supyrb">
//   Copyright (c) 2020 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

namespace Supyrb
{
	/// <summary>
	/// Base class for Signals
	/// Provides a hash and parameterless constructor
	/// </summary>
	public abstract class ABaseSignal
	{
		private string _hash;

		/// <summary>
		/// Unique id for this signal
		/// </summary>
		public string Hash
		{
			get
			{
				if (string.IsNullOrEmpty(_hash))
				{
					_hash = this.GetType().ToString();
				}

				return _hash;
			}
		}

		protected ABaseSignal()
		{
		}
	}
}