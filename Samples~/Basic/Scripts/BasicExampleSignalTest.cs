// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleSignalTest.cs" company="Supyrb">
//   Copyright (c)  Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Supyrb
{
	public class BasicExampleSignalTest : MonoBehaviour
	{
		private Signal exampleSignal;
		private void Awake()
		{
			exampleSignal = Signals.Get<BasicExampleSignal>();
			SubscribeListeners();
		}

		private void Start()
		{
			DispatchSignal();
		}

		private void SubscribeListeners()
		{
			exampleSignal.AddListener(FirstListener, -100);
			exampleSignal.AddListener(PauseTwoSecondsListener, -10);
			exampleSignal.AddListener(DefaultListener);
			exampleSignal.AddListener(ConsumeEventListener, 10);
			exampleSignal.AddListener(LastListener, 100);
		}
		
		private void OnDestroy()
		{
			exampleSignal.RemoveListener(FirstListener);
			exampleSignal.RemoveListener(PauseTwoSecondsListener);
			exampleSignal.RemoveListener(DefaultListener);
			exampleSignal.RemoveListener(ConsumeEventListener);
			exampleSignal.RemoveListener(LastListener);
		}
		
		[ContextMenu("DispatchSignal")]
		public void DispatchSignal()
		{
			exampleSignal.Dispatch();
		}
		
		private void FirstListener()
		{
			Debug.Log("First Listener (Order -100)");
		}

		private void PauseTwoSecondsListener()
		{
			Debug.Log("Pausing for 2 seconds (Order -10)");
			exampleSignal.Pause();
			StartCoroutine(ContinueAfterDelay(exampleSignal, 2f));
		}

		private void DefaultListener()
		{
			Debug.Log("Default order Listener (Order 0)");
		}

		private void ConsumeEventListener()
		{
			Debug.Log("Consume Signal (Order 10)");
			exampleSignal.Consume();
		}

		private void LastListener()
		{
			Debug.Log("Won't be called, since the signal was consumed. (Order 100)");
		}

		private IEnumerator ContinueAfterDelay(Signal signal, float delay)
		{
			yield return new WaitForSeconds(delay);
			signal.Continue();
		}
	}
}