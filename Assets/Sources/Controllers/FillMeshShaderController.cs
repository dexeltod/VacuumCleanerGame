using System;
using Sources.ControllersInterfaces;
using UnityEngine;

namespace Sources.Controllers
{
	public class FillMeshShaderController : IFillMeshShaderController
	{
		private const float NormalizeConst = 1;
		private const string FillAreaShaderName = "_FillAmount";

		private readonly MeshRenderer _renderer;

		private readonly float _minFillArea;
		private readonly float _maxFillArea;

		private int FillAreaShaderPropertyId => Shader.PropertyToID(FillAreaShaderName);

		public FillMeshShaderController(MeshRenderer renderer) =>
			_renderer = renderer ? renderer : throw new ArgumentNullException(nameof(renderer));

		public void FillArea(float originalCount, float minFillCount, float maxFillCount)
		{
			if (originalCount < minFillCount)
				throw new ArgumentOutOfRangeException($"Value {originalCount} is less than minFillArea {minFillCount}");

			float normalized = Normalize(originalCount, minFillCount, maxFillCount);
			_renderer.material.SetFloat(FillAreaShaderPropertyId, normalized);
		}

		private float Normalize(float originalValue, float min, float max) =>
			(originalValue - min) / (max - min);
	}
}