using System;
using Sources.UseCases.Scene;
using UnityEngine;

namespace Sources.Infrastructure.Factories.Coroutine
{
	public class CoroutineRunner : MonoBehaviour, ICoroutineRunner
	{
		public void StopCoroutineRunning(UnityEngine.Coroutine coroutine)
		{
			if (coroutine == null)
				throw new ArgumentNullException(nameof(coroutine));

			StopCoroutine(coroutine);
		}
	}
}