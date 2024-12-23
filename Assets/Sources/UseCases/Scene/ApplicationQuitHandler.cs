using System;
using UnityEngine;

namespace Sources.UseCases.Scene
{
	public class ApplicationQuitHandler : MonoBehaviour
	{
		public event Action ApplicationClosed;

		private void Awake() =>
			DontDestroyOnLoad(this);

		private void OnApplicationQuit() =>
			ApplicationClosed?.Invoke();
	}
}