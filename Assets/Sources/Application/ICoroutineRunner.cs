using System.Collections;
using UnityEngine;

namespace Sources.Application
{
	public interface ICoroutineRunner
	{
		Coroutine StartCoroutine(IEnumerator routine);
	}
}