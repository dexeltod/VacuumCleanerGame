using System.Collections;
using UnityEngine;

namespace Sources.Core
{
	public interface ICoroutineRunner
	{
		Coroutine StartCoroutine(IEnumerator routine);
	}
}