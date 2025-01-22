using System.Collections;
using UnityEngine;

namespace Sources.Utils
{
	public interface ICoroutineRunner
	{
		Coroutine Run(string methodName);
		Coroutine Run(IEnumerator enumerator);
		void StopCoroutineRunning(Coroutine coroutine);
	}
}