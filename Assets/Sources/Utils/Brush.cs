using System;
using UnityEngine;

namespace Sources.Utils
{
	public class Brush : MonoBehaviour
	{
		[SerializeField] private CustomRenderTexture _heightMap;
		[SerializeField] private Material _heightMapMaterial;
		[SerializeField] private Camera _mainCamera;
		private static readonly int DrawPosition = Shader.PropertyToID("_DrawPosition");

		private void Awake()
		{
			_heightMap.Initialize();
		}

		private void Update()
		{
			// Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

			// if (Physics.Raycast(ray, out RaycastHit hit))
			// {
			// 	_heightMapMaterial.SetVector(DrawPosition, hit.textureCoord);
			// }
		}
	}
}