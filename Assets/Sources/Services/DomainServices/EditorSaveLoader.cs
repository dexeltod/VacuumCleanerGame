using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sources.ApplicationServicesInterfaces;
using Sources.Domain.Progress;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.DomainServicesInterfaces;
using Unity.Services.CloudSave;
using UnityEngine;

public class EditorSaveLoader : ISaveLoader
{
	private const string GameProgressKey = "GameProgress";

	private readonly IPersistentProgressService _progressService;
	private readonly IUnityServicesController _unityServicesController;

	public EditorSaveLoader(IPersistentProgressService progressService,
		IUnityServicesController unityServicesController)
	{
		_progressService = progressService;
		_unityServicesController = unityServicesController;
	}

	public async UniTask Initialize() =>
		await _unityServicesController.InitializeUnityServices();

	public async void Save(IGameProgressModel @object)
	{
		GameProgressModel model = _progressService.GameProgress as GameProgressModel;
		string dataJsonUtility = JsonUtility.ToJson(model);

		await CloudSaveService.Instance.Data.ForceSaveAsync(new Dictionary<string, object>
			{
				{ GameProgressKey, dataJsonUtility }
			}
		);
	}

	public async UniTask<IGameProgressModel> Load()
	{
		Dictionary<string, string> keyAndJsonSaves = await CloudSaveService
			.Instance
			.Data
			.LoadAsync
			(
				new HashSet<string> { GameProgressKey }
			);

		string jsonSave = keyAndJsonSaves.Values.LastOrDefault();

		return DeserializeJson(jsonSave);
	}

	private IGameProgressModel DeserializeJson(string jsonSave)
	{
		try
		{
			GameProgressModel model = JsonUtility.FromJson<GameProgressModel>(jsonSave);

			return model;
		}
		catch (Exception e)
		{
			Debug.LogException(e);
			Debug.Log("New progress will be created");
			return null;
		}
	}
}