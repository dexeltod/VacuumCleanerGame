using AYellowpaper;
using Sources.Domain.Progress;
using UnityEngine;

namespace Sources.Application
{
	public class GameRunner : MonoBehaviour
	{
		[SerializeField] private Bootstrapper _bootstrapperPrefab;

		private void Awake()
		{
			var bootstrapper = FindObjectOfType<Bootstrapper>();

			if (bootstrapper == null) 
				Instantiate(_bootstrapperPrefab);
		}
	}
}