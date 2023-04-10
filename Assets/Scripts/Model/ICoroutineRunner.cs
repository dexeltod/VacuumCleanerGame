using System.Collections;
using UnityEngine;

namespace Model
{
	public interface ICoroutineRunner
	{
		Coroutine StartCoroutine(IEnumerator routine);
	}
}