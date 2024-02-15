using System.Collections;
using UnityEngine;

namespace Sources.Utils
{
	public interface ICoroutineRunner
	{
		void StopCoroutineRunning(UnityEngine.Coroutine coroutine);
		Coroutine Run(string methodName);
		Coroutine Run(IEnumerator enumerator);
	}
}