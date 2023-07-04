using System.Collections;
using UnityEngine;

namespace InfrastructureInterfaces
{
	public interface ICoroutineRunner
	{
		Coroutine StartCoroutine(IEnumerator routine);
	}
}