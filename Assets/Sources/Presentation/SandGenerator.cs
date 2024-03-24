using UnityEngine;

namespace Sources.Presentation
{
	public class SandGenerator : MonoBehaviour
	{
		[SerializeField] private float _cellSize = 0.2f;

		[SerializeField] private int _slopeLength;
		[SerializeField] private float _middleHeight;

		public float CellSize => _cellSize;
		public int SlopeLength => _slopeLength;
		public float MiddleHeight => _middleHeight;
	}
}