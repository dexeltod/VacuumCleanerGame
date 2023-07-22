using UnityEngine;

namespace Sources.Core.Application
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