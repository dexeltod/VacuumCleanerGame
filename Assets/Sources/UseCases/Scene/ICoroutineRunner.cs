using System.Collections;
using UnityEngine;

namespace Sources.UseCases.Scene
{
	public interface ICoroutineRunner
	{
		Coroutine StartCoroutine(IEnumerator routine);
		void StopCoroutineRunning(Coroutine coroutine);
	}
}