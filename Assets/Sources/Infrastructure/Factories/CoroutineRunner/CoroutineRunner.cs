using System;
using System.Collections;
using Sources.UseCases.Scene;
using UnityEngine;

namespace Sources.Infrastructure.Factories.CoroutineRunner
{
	public class CoroutineRunner : MonoBehaviour, ICoroutineRunner
	{
		public void StopCoroutineRunning(Coroutine coroutine)
		{
			if (coroutine == null)
				throw new ArgumentNullException(nameof(coroutine));

			StopCoroutine(coroutine);
		}

		Coroutine ICoroutineRunner.Run(string methodName) =>
			StartCoroutine(methodName);

		public Coroutine Run(IEnumerator enumerator) =>
			enumerator != null ? StartCoroutine(enumerator) : null;
	}
}