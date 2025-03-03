using Sources.Presentation.SceneEntity;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.Presentation
{
	public class ResourceTriggerHandler : MonoBehaviour
	{
		[FormerlySerializedAs("_view")]
		[SerializeField]
		private ResourcePresentation _presentation;

		private void OnTriggerEnter(Collider other)
		{
			if (_presentation != null) _presentation.HandleCollide(other);
		}
	}
}
