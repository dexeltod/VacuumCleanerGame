using Model;
using UnityEngine;

namespace ViewModel.Infrastructure
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