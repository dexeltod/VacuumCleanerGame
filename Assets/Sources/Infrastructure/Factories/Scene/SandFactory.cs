using System;
using System.Collections.Generic;
using Sources.Controllers.Mesh;
using Sources.InfrastructureInterfaces.Providers;
using Sources.Presentation;
using Sources.Presentation.SceneEntity;
using Sources.PresentationInterfaces;
using Sources.Services;
using Sources.ServicesInterfaces;
using Sources.Utils.Configs.Scripts;
using UnityEngine;

namespace Sources.Infrastructure.Factories.Scene
{
	public class SandFactory
	{
		private const float Error = 0.9f;
		private const string MeshName = "Grid";
		private readonly IAssetFactory _assetFactory;
		private readonly ILevelProgressFacade _levelProgressFacade;
		private readonly IResourcesProgressPresenterProvider _resourcesProgressPresenterProvider;
		private readonly Mesh _mesh = new Mesh();
		private readonly Dictionary<int, MeshCollider> _meshCollidersById = new();

		private MeshCollider _meshCollider;
		private MeshFilter _meshFilter;

		private Texture2D _deformTexture;
		private Renderer _renderer;
		private Bounds _bounds;

		private Vector3[] _vertices;
		private Vector2[] _uvs;
		private float _maxY;
		private float _minY;

		private GameObject _gameObject;

		private float _middleHeight;
		private float _cellSize;
		private int _slopeLength;
		private int[] _triangles;
		private int _textureResolution;

		private Material _deformMaterial;
		private Material _sandMaterial;
		private MeshDeformationPresenter _controller;
		private MeshModificator _meshModificator;

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

		private string GameObjectsSandGround => GameObjects.SandGround;
		private string SandMaterial => GameObjects.SandMaterial;
		private int HalfSandSize => (int)Math.Ceiling(_levelProgressFacade.MaxScoreCount * Error);

		public IMeshModifiable Create()
		{
			_gameObject = _assetFactory.Instantiate(GameObjectsSandGround);
			SandGenerator sandGenerator = _gameObject.GetComponent<SandGenerator>();

			_sandMaterial = _assetFactory.LoadFromResources<Material>(SandMaterial);

			_meshModificator = _gameObject.GetComponent<MeshModificator>();
			_meshCollider = _gameObject.GetComponent<MeshCollider>();

			SetFieldsFromComponent(sandGenerator);

			_meshCollider.sharedMesh = _mesh;
			_meshModificator.GetComponent<MeshCollider>();

			_controller = new MeshDeformationPresenter(
				_resourcesProgressPresenterProvider,
				_meshModificator.RadiusDeformation,
				_meshCollidersById
			);

			GenerateChunks();

			return _meshModificator;
		}

		private void GenerateChunks()
		{
			int currentChunk = 0;
			const int ChunkSize = 50; // Размер чанка по вершинам (можно настроить)

			int chunkCountX = HalfSandSize / ChunkSize;
			int chunkCountZ = HalfSandSize / ChunkSize;

			for (int chunkZ = 0; chunkZ < chunkCountZ; chunkZ++)
			{
				for (int chunkX = 0; chunkX < chunkCountX; chunkX++)
				{
					GameObject chunkObject = new GameObject("Chunk_" + chunkX + "_" + chunkZ);
					MeshFilter chunkMeshFilter = chunkObject.AddComponent<MeshFilter>();
					DeformableChunk deformableChunk = chunkObject.AddComponent<DeformableChunk>();

					var collider = chunkObject.AddComponent<MeshCollider>();
					MeshRenderer chunkMeshRenderer = chunkObject.AddComponent<MeshRenderer>();
					collider.sharedMesh = chunkMeshFilter.mesh;
					chunkMeshRenderer.sharedMaterial = _sandMaterial;

					int startX = chunkX * ChunkSize;
					int startZ = chunkZ * ChunkSize;

					const int VerticesCount = (ChunkSize + 1) * (ChunkSize + 1);

					Vector3[] chunkVertices = new Vector3[VerticesCount];
					Vector2[] chunkUVs = new Vector2[VerticesCount];

					int[] chunkTriangles = new int[ChunkSize * ChunkSize * 6];

					for (int z = 0, vi = 0; z <= ChunkSize; z++)
					{
						for (int x = 0; x <= ChunkSize; x++, vi++)
						{
							SetVertices(startX, x, startZ, z, chunkVertices, vi, chunkUVs);
						}
					}

					Mesh mesh = chunkMeshFilter.mesh;
					mesh.vertices = chunkVertices;
					mesh.uv = chunkUVs;

					int ti = 0;

					SetTriangles(ChunkSize, ti, chunkTriangles);

					chunkMeshFilter.mesh.triangles = chunkTriangles;

					PlaceChunkIntoParent(chunkObject);

					deformableChunk.Construct(
						_meshModificator.RadiusDeformation,
						_meshModificator.PointPerOneSand,
						_resourcesProgressPresenterProvider,
						_controller,
						currentChunk
					);
					
					_meshCollidersById.Add(currentChunk, collider);
					
					currentChunk++;
				}
			}
		}

		private void SetVertices(
			int startX,
			int x,
			int startZ,
			int z,
			Vector3[] chunkVertices,
			int vi,
			Vector2[] chunkUVs
		)
		{
			int globalX = startX + x;
			int globalZ = startZ + z;

			float y = SetHeight(globalX, globalZ);

			chunkVertices[vi] = new Vector3(
				globalX * _cellSize,
				y * _cellSize,
				globalZ * _cellSize
			);
			chunkUVs[vi] = new Vector2((float)globalX / HalfSandSize, (float)globalZ / HalfSandSize);
		}

		private static void SetTriangles(int chunkSize, int ti, int[] chunkTriangles)
		{
			for (int z = 0, vi = 0; z < chunkSize; z++, vi++)
			{
				for (int x = 0; x < chunkSize; x++, ti += 6, vi++)
				{
					chunkTriangles[ti] = vi;
					chunkTriangles[ti + 1] = chunkTriangles[ti + 4] = vi + chunkSize + 1;
					chunkTriangles[ti + 2] = chunkTriangles[ti + 3] = vi + 1;
					chunkTriangles[ti + 5] = vi + chunkSize + 2;
				}
			}
		}

		private void PlaceChunkIntoParent(GameObject chunkObject)
		{
			chunkObject.transform.position = _gameObject.transform.position;
			chunkObject.transform.parent = _gameObject.transform;
		}

		private float SetHeight(int x, int z)
		{
			float y;

			if (x < _slopeLength || z < _slopeLength || z > HalfSandSize - _slopeLength ||
				x > HalfSandSize - _slopeLength)
				y = 0;
			else
				y = _middleHeight;

			return y;
		}

		private void SetFieldsFromComponent(SandGenerator sandGenerator)
		{
			_slopeLength = sandGenerator.SlopeLength;
			_middleHeight = sandGenerator.MiddleHeight;
			_cellSize = sandGenerator.CellSize;
		}
	}
}