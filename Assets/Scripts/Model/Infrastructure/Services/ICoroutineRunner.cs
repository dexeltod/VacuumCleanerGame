using System.Collections;
using UnityEngine;

namespace Model.Infrastructure.Services
{
	public interface ICoroutineRunner
	{
		Coroutine StartCoroutine(IEnumerator routine);
	}
}