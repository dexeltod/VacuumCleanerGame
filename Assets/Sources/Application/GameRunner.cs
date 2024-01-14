using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.Application
{
	public class GameRunner : MonoBehaviour
	{
		[FormerlySerializedAs("_bootstrapperPrefab")] [SerializeField] private Boot _bootPrefab;

		private void Awake()
		{
			Boot boot = FindObjectOfType<Boot>();

			if (boot == null)
				Instantiate(_bootPrefab);
		}
	}
}