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
	public class BasicExampleSignalTest : MonoBehaviour
	{
		[SerializeField]
		private bool logListeners = true;
		
		private BasicExampleSignal basicExampleSignal;
		private void Awake()
		{
			Signals.Get(out basicExampleSignal);
			SubscribeListeners();
		}

		private void Start()
		{
			DispatchSignal();
		}

		private void SubscribeListeners()
		{
			basicExampleSignal.AddListener(FirstListener, -100);
			basicExampleSignal.AddListener(PauseTwoSecondsListener, -10);
			basicExampleSignal.AddListener(DefaultListener);
			basicExampleSignal.AddListener(ConsumeEventListener, 10);
			basicExampleSignal.AddListener(LastListener, 100);
		}
		
		private void OnDestroy()
		{
			basicExampleSignal.RemoveListener(FirstListener);
			basicExampleSignal.RemoveListener(PauseTwoSecondsListener);
			basicExampleSignal.RemoveListener(DefaultListener);
			basicExampleSignal.RemoveListener(ConsumeEventListener);
			basicExampleSignal.RemoveListener(LastListener);
		}
		
		[ContextMenu("DispatchSignal")]
		public void DispatchSignal()
		{
			basicExampleSignal.Dispatch();
		}
		
		private void FirstListener()
		{
			ConditionalLog("First Listener (Order -100)");
		}

		private void PauseTwoSecondsListener()
		{
			ConditionalLog("Pausing for 2 seconds (Order -10)");
			basicExampleSignal.Pause();
			StartCoroutine(ContinueAfterDelay(basicExampleSignal, 2f));
		}

		private void DefaultListener()
		{
			ConditionalLog("Default order Listener (Order 0)");
		}

		private void ConsumeEventListener()
		{
			ConditionalLog("Consume Signal (Order 10)");
			basicExampleSignal.Consume();
		}

		private void LastListener()
		{
			ConditionalLog("Won't be called, since the signal was consumed. (Order 100)");
		}

		private IEnumerator ContinueAfterDelay(Signal signal, float delay)
		{
			yield return new WaitForSeconds(delay);
			signal.Continue();
		}

		private void ConditionalLog(string text)
		{
			if (logListeners)
			{
				Debug.Log(text, this);
			}
		}
	}
}