using System;
using Sources.Application.StateMachine;
using Sources.DIService;
using Sources.Presentation.SceneEntity;
using Sources.Services;
using Sources.ServicesInterfaces;
using Sources.UseCases.Scene;
using Sources.Utils.Configs.Scripts;
using UnityEngine;

namespace Sources.Application
{
	public class Bootstrapper : MonoBehaviour, ICoroutineRunner
	{
		private Game _game;

		private void Awake()
		{
			DontDestroyOnLoad(this);

			IAssetProvider assetProvider = new AssetProvider();

			LoadingCurtain loadingCurtain = LoadLoadingCurtain(assetProvider);
			loadingCurtain.gameObject.SetActive(true);

			SceneLoader sceneLoader = new SceneLoader();

			ServiceLocator.Initialize();

			GameStateMachine gameStateMachine = new GameStateMachineFactory(
				sceneLoader,
				assetProvider,
				loadingCurtain,
				this
			).Create();

			_game = new Game(gameStateMachine);
			_game.Start();
		}

		private LoadingCurtain LoadLoadingCurtain(IAssetProvider assetProvider) =>
			assetProvider.InstantiateAndGetComponent<LoadingCurtain>(ResourcesAssetPath.GameObjects.LoadinCrutain);

		public void StopCoroutineRunning(Coroutine coroutine)
		{
			if (coroutine == null) throw new ArgumentNullException(nameof(coroutine));

			StopCoroutine(coroutine);
		}
	}
}