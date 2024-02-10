using System;
using System.Collections;
using UnityEngine;

namespace Sources.UseCases.Scene
{
	public interface ICoroutineRunner
	{
		void StopCoroutineRunning(UnityEngine.Coroutine coroutine);
		Coroutine Run(string methodName);
		Coroutine Run(IEnumerator enumerator);
	}
}