using System;
using Sources.ControllersInterfaces;
using UnityEngine;

namespace Sources.Controllers
{
	public class FillMeshShader : IFillMeshShader
	{
		private const float NormalizeConst = 1;
		private const string FillAreaShaderName = "_FillAmount";
		private readonly float _maxFillArea;

		private readonly float _minFillArea;

		private readonly MeshRenderer _renderer;

		public FillMeshShader(MeshRenderer renderer) =>
			_renderer = renderer ? renderer : throw new ArgumentNullException(nameof(renderer));

		private int FillAreaShaderPropertyId => Shader.PropertyToID(FillAreaShaderName);

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