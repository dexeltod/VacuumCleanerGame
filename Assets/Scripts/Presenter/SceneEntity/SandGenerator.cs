using UnityEngine;

namespace Model.Character
{
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	public class SandGenerator : MonoBehaviour
	{
		[SerializeField] private int _size = 2;
		[SerializeField] private Vector2 _noise;
		[SerializeField] private Gradient _colorHeight;
		[SerializeField] private float _perlinNoiseHeight = 1f;
		[SerializeField] private float _middleHeight;
		[SerializeField] private float _slopeLenght;

		private const int CellSize = 6;
		
		private Collider _collider;
		private float _minY;
		private float _maxY;

		private Mesh _mesh;
		private Vector3[] _vertices;
		private int[] _triangles;
		private Color[] _colorLevels;
		private Vector2[] _uvs;

		private MeshFilter _filter;
		private MeshRenderer _meshRenderer;

		private void Start()
		{
			_mesh = new Mesh();
			_mesh = GetComponent<MeshFilter>().mesh;
			_meshRenderer = GetComponent<MeshRenderer>();

			StartGeneration();
			UpdateCollider();
		}

		private void StartGeneration()
		{
			//Установление длинн массивов
			_vertices = new Vector3[(_size + 1) * (_size + 1)];
			_triangles = new int[_size * _size * CellSize];

			SetVertices();

			//Разукраска вершин
			// DrawMesh();

			//Создание треугольников
			int trianStep = 0;
			int vertStep = 0;
			for (int z = 0; z < _size; z++)
			{
				for (int x = 0; x < _size; x++)
				{
					_triangles[0 + trianStep] = vertStep;
					_triangles[1 + trianStep] = vertStep + _size + 1;
					_triangles[2 + trianStep] = vertStep + 1;
					_triangles[3 + trianStep] = vertStep + 1;
					_triangles[4 + trianStep] = vertStep + _size + 1;
					_triangles[5 + trianStep] = vertStep + _size + 2;
					trianStep += 6;
					vertStep++;
				}

				vertStep++;
			}

			UpdateMesh();
		}

		private void SetVertices()
		{
			for (int i = 0, z = 0; z <= _size; z++)
			{
				int huy = _size * (30 / 100);

				for (int x = 0; x <= _size; x++)
				{
					float y;

					if (x < _slopeLenght || z < _slopeLenght || z > _size - _slopeLenght || x > _size - _slopeLenght)
						y = 0;
					else
						y = Mathf.PerlinNoise(_noise.x * x, _noise.y * z) * _perlinNoiseHeight +
						    _middleHeight;

					_vertices[i] = new Vector3(x, y, z) + transform.position;
					if (y > _maxY) _maxY = y;
					if (y < _minY) _minY = y;
					i++;
				}
			}
		}

		private void DrawMesh()
		{
			_colorLevels = new Color[_vertices.Length];
			_uvs = new Vector2[_vertices.Length];

			for (int v = 0; v < _vertices.Length; v++)
			{
				//Процентное соотношение
				float heghts = Mathf.InverseLerp(_minY, _maxY, _vertices[v].y - transform.position.y);
				//Установка цвета для вершины
				_colorLevels[v] = _colorHeight.Evaluate(heghts);
			}

			Texture2D tx2d = new Texture2D(_size, _size);
			for (int x = 0, i = 0; x <= _size; x++)
			{
				for (int y = 0; y <= _size; y++)
				{
					_uvs[i] = new Vector2((float)x / _size, (float)y / _size);
					tx2d.SetPixel(x, y, _colorLevels[i]);
					i++;
				}
			}

			tx2d.Apply();
			_meshRenderer.sharedMaterial.mainTexture = tx2d;
		}

		private void UpdateCollider()
		{
			_collider = GetComponent<Collider>();
			Destroy(_collider);
			MeshCollider component = gameObject.AddComponent<MeshCollider>();
		}

		private void UpdateMesh()
		{
			_mesh.vertices = _vertices;
			_mesh.triangles = _triangles;
			_mesh.uv = _uvs;
			_mesh.RecalculateNormals();
		}
	}
}