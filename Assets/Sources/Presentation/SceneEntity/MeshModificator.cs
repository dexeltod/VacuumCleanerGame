using Sources.PresentationInterfaces;
using UnityEngine;

namespace Sources.Presentation.SceneEntity
{
	public class MeshModificator : MonoBehaviour, IMeshModifiable
	{
		private const string VacuumColliderBottom = "VacuumColliderBottom";
		private const string VacuumColliderTop = "VacuumColliderTop";

		[SerializeField] private float _radiusDeformation = 2;
		[SerializeField] private int _pointPerOneSand = 1;

		public float RadiusDeformation => _radiusDeformation;

		public int PointPerOneSand => _pointPerOneSand;
	}
}