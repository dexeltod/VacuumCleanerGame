using System;
using System.Collections;
using Sources.Utils;
using UnityEngine;

namespace Sources.Infrastructure.CoroutineRunner
{
	public class CoroutineRunner : MonoBehaviour, ICoroutineRunner
	{
		private void Awake()
		{
			DontDestroyOnLoad(this);
		}

		public void StopCoroutineRunning(Coroutine coroutine)
		{
			if (coroutine == null)
				throw new ArgumentNullException(nameof(coroutine));

			StopCoroutine(coroutine);
		}

		Coroutine ICoroutineRunner.Run(string methodName) => StartCoroutine(methodName);

		public Coroutine Run(IEnumerator enumerator) => enumerator != null ? StartCoroutine(enumerator) : null;
	}
}