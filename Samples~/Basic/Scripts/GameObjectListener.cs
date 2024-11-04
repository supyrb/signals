// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleSignalTest.cs" company="Supyrb">
//   Copyright (c)  Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;

namespace Supyrb
{
    public class GameObjectListener : MonoBehaviour
	{
		private void OnEnable()
		{
			Signals.Get<GameObjectSignal>().AddListener(OnSignalFired);
		}

		private void OnDisable()
		{
			Signals.Get<GameObjectSignal>().RemoveListener(OnSignalFired);
		}

		private void OnSignalFired(GameObject go)
		{
			Debug.Log("Received signal with game object: " + go.name);
		}
	}
}