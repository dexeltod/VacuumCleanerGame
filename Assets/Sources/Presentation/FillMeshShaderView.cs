using Sources.PresentationInterfaces;
using UnityEngine;

namespace Sources.Presentation
{
	public class FillMeshShaderView : MonoBehaviour, IFillMeshShaderView
	{
		[SerializeField] private MeshRenderer _meshRenderer;


		[SerializeField, Range(-1, 2)] private float _minFillArea = 0;
		[SerializeField, Range(-1, 2)] private float _maxFillArea = 1;

		public MeshRenderer MeshRenderer => _meshRenderer;
		public float MaxFillArea => _maxFillArea;
		public float MinFillArea => _minFillArea;
	}
}