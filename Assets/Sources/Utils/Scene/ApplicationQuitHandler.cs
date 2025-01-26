using System;
using UnityEngine;

namespace Sources.Utils.Scene
{
	public class ApplicationQuitHandler : MonoBehaviour
	{
		private void Awake() => DontDestroyOnLoad(this);

		private void OnApplicationQuit() => ApplicationClosed?.Invoke();

		public event Action ApplicationClosed;
	}
}