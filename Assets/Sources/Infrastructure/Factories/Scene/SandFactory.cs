using System;
using Sources.Controllers.Mesh;
using Sources.Infrastructure.Configs.Scripts;
using Sources.InfrastructureInterfaces.Providers;
using Sources.Presentation;
using Sources.Presentation.SceneEntity;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using Sources.Utils;
using UnityEngine;

namespace Sources.Infrastructure.Factories.Scene
{
	public class SandFactory
	{
		private const float Error = 0.9f;

		private readonly IAssetFactory _assetFactory;
		private readonly ILevelProgressFacade _levelProgressFacade;
		private readonly IResourcesProgressPresenterProvider _resourcesProgressPresenterProvider;

		private MeshCollider _meshCollider;
		private MeshFilter _meshFilter;
		private readonly Mesh _mesh = new Mesh();

		private string _meshName = "Grid";

		private Vector3[] _vertices;
		private Vector2[] _uvs;
		private float _maxY;
		private float _minY;
		private int[] _triangles;

		private GameObject _gameObject;

		private int _slopeLenght;
		private float _middleHeight;
		private float _cellSize;

		public SandFactory(
			IAssetFactory assetFactory,
			ILevelProgressFacade levelProgressFacade,
			IResourcesProgressPresenterProvider resourcesProgressPresenterProvider
		)
		{
			_assetFactory = assetFactory ?? throw new ArgumentNullException(nameof(assetFactory));
			_levelProgressFacade = levelProgressFacade ?? throw new ArgumentNullException(nameof(levelProgressFacade));
			_resourcesProgressPresenterProvider = resourcesProgressPresenterProvider ??
				throw new ArgumentNullException(nameof(resourcesProgressPresenterProvider));
		}

		private string GameObjectsSandGround => ResourcesAssetPath.GameObjects.SandGround;
		private int HalfSandSize => (int)Math.Ceiling(_levelProgressFacade.MaxTotalResourceCount * Error / 2f);

		public IMeshModifiable Create()
		{
			_gameObject = _assetFactory.Instantiate(GameObjectsSandGround);

			MeshView meshView = _gameObject.GetComponent<MeshView>();
			SandGenerator sandGenerator = _gameObject.GetComponent<SandGenerator>();
			_meshCollider = _gameObject.GetComponent<MeshCollider>();

			SetFieldsFromComponent(sandGenerator);

			Generate();
			_meshCollider.sharedMesh = _mesh;

			var controller = new MeshDeformationPresenter(
				_mesh,
				_resourcesProgressPresenterProvider.Implementation,
				meshView.RadiusDeformation,
				meshView.GetComponent<MeshCollider>()
			);

			meshView.Construct(_resourcesProgressPresenterProvider, controller);
			return meshView;
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
			_vertices = new Vector3[(HalfSandSize + 1) * (HalfSandSize + 1)];

			Vector2[] uvs = new Vector2[_vertices.Length];
			Vector4[] tangents = new Vector4[_vertices.Length];
			Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

			for (int i = 0, z = 0; z <= HalfSandSize; z++)
			{
				for (int x = 0; x <= HalfSandSize; x++, i++)
				{
					_vertices[i] = new Vector3(x, z);
					uvs[i] = new Vector2((float)x / HalfSandSize, (float)z / HalfSandSize);
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
			float y;

			if (x < _slopeLenght || z < _slopeLenght || z > HalfSandSize - _slopeLenght ||
				x > HalfSandSize - _slopeLenght)
				y = 0;
			else
				y = _middleHeight;
			return y;
		}

		private void CreateTriangles()
		{
			const int TrianglesEdges = 6;

			_triangles = new int[HalfSandSize * HalfSandSize * TrianglesEdges];

			for (int ti = 0, vi = 0, z = 0; z < HalfSandSize; z++, vi++)
			{
				for (int x = 0; x < HalfSandSize; x++, ti += TrianglesEdges, vi++)
				{
					_triangles[ti] = vi;
					_triangles[ti + 1] = _triangles[ti + 4] = vi + HalfSandSize + 1;
					_triangles[ti + 2] = _triangles[ti + 3] = vi + 1;
					_triangles[ti + 5] = vi + HalfSandSize + 2;
				}
			}

			_mesh.triangles = _triangles;
		}

		private void CreateMesh()
		{
			_gameObject.GetComponent<MeshFilter>().mesh = _mesh;
			_mesh.name = _meshName;
		}

		private void SetFieldsFromComponent(SandGenerator sandGenerator)
		{
			_slopeLenght = sandGenerator.SlopeLength;
			_middleHeight = sandGenerator.MiddleHeight;
			_cellSize = sandGenerator.CellSize;
		}
	}
}