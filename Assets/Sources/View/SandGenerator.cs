using UnityEngine;

namespace Sources.View
{
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	[RequireComponent(typeof(MeshCollider))]
	public class SandGenerator : MonoBehaviour
	{
		[SerializeField] private float _cellSize = 0.2f;
		[SerializeField] private int _xSize;
		[SerializeField] private int _zSize;
		[SerializeField] private int _slopeLenght;
		[SerializeField] private float _middleHeight;
		[SerializeField] private bool _isDebug;

		private Mesh _mesh;
		private MeshFilter _meshFilter;
		private float _maxY;
		private float _minY;
		private int[] _triangles;
		private Vector3[] _vertices;
		private Vector2[] _uvs;
		private MeshCollider _meshCollider;

		private void Start()
		{
			_meshCollider = GetComponent<MeshCollider>();

			if (_zSize <= 0 || _xSize <= 0)
				return;

			Generate();
			_meshCollider.sharedMesh = _mesh;
		}

		private void Generate()
		{
			CreateMesh();
			CreateVertices();
			CreateTriangles();
			_mesh.RecalculateNormals();
		}

		private void CreateVertices()
		{
			_vertices = new Vector3[(_xSize + 1) * (_zSize + 1)];

			Vector2[] uvs = new Vector2[_vertices.Length];
			Vector4[] tangents = new Vector4[_vertices.Length];
			Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

			for (int i = 0, z = 0; z <= _zSize; z++)
			{
				for (int x = 0; x <= _xSize; x++, i++)
				{
					_vertices[i] = new Vector3(x, z);
					uvs[i] = new Vector2((float)x / _xSize, (float)z / _zSize);
					tangents[i] = tangent;

					var y = SetHeight(x, z);

					_vertices[i] = new Vector3(x * _cellSize, y * _cellSize, z * _cellSize);
				}
			}

			_mesh.vertices = _vertices;

			_mesh.uv = uvs;
			_mesh.tangents = tangents;
		}

		private float SetHeight(int x, int z)
		{
			float y = 0;
			if (x < _slopeLenght || z < _slopeLenght || z > _zSize - _slopeLenght ||
			    x > _xSize - _slopeLenght)
				y = 0;
			else
				y = _middleHeight;
			return y;
		}

		private void CreateTriangles()
		{
			_triangles = new int[_xSize * _zSize * 6];

			for (int ti = 0, vi = 0, z = 0; z < _zSize; z++, vi++)
			{
				for (int x = 0; x < _xSize; x++, ti += 6, vi++)
				{
					_triangles[ti] = vi;
					_triangles[ti + 1] = _triangles[ti + 4] = vi + _xSize + 1;
					_triangles[ti + 2] = _triangles[ti + 3] = vi + 1;
					_triangles[ti + 5] = vi + _xSize + 2;
				}
			}

			_mesh.triangles = _triangles;
		}

		private void CreateMesh()
		{
			_mesh = new Mesh();
			GetComponent<MeshFilter>().mesh = _mesh;
			_mesh.name = "Grid";
		}
	}
}