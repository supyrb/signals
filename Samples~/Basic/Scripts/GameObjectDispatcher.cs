// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleSignalTest.cs" company="Supyrb">
//   Copyright (c)  Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;
using UnityEngine;

namespace Supyrb
{
	public class GameObjectDispatcher : MonoBehaviour
	{
		private IEnumerator Start()
		{
			yield return new WaitForSeconds(1f);
			Signals.Get<GameObjectSignal>().Dispatch(gameObject);
		}
	}
}