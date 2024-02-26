using System;
using Sources.PresentationInterfaces;
using UnityEngine;

namespace Sources.Presentation.SceneEntity
{
	public class SandCarContainerView : MonoBehaviour, ISandContainerView
	{
		[SerializeField] private Vector3 _maxPosition;
		[SerializeField] private Vector3 _minPosition;

		[SerializeField] private Vector3 _maxScale;
		[SerializeField] private Vector3 _minScale;

		[SerializeField] private GameObject _sand;

		[SerializeField, Range(0, 1)] private float _sandValueSliderTest;

		[SerializeField] private bool _isDebug;

		private float _normalizedCount = 0;

		private void OnValidate()
		{
			if (_isDebug)
				SetSand(_sandValueSliderTest);
		}

		public void SetSand(float normalizedCount)
		{
			_sand.gameObject.SetActive(_normalizedCount > 0);

			_normalizedCount = normalizedCount;

			if (normalizedCount < 0 || normalizedCount > 1)
				throw new ArgumentOutOfRangeException(nameof(normalizedCount));

			_sand.transform.localPosition = Vector3.Lerp(_minPosition, _maxPosition, normalizedCount);
			Vector3 scale = Vector3.Lerp(_minScale, _maxScale, normalizedCount);
			_sand.transform.localScale = scale;
		}
	}
}