using System.Collections;
using UnityEngine;

namespace ViewModel.Infrastructure.Services
{
	public interface ICoroutineRunner
	{
		Coroutine StartCoroutine(IEnumerator routine);
	}
}