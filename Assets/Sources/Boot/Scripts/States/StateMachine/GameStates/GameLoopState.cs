using System;
using Cysharp.Threading.Tasks;
using Sources.BusinessLogic.Repository;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.BusinessLogic.States.StateMachineInterfaces;
using Sources.Controllers;
using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.PresentationInterfaces;
using VContainer;

namespace Sources.Boot.Scripts.States.StateMachine.GameStates
{
	public sealed class GameLoopState : IGameState
	{
		private readonly ILocalizationService _localizationService;
		private readonly IResourcesProgressPresenter _resourcesProgressPresenter;
		private readonly IDissolveShaderViewController _dissolveShaderViewController;

		private readonly IPersistentProgressService _persistentProgressService;
		private readonly IActiveRepository<IPresenter> _presentersRepository;
#if YANDEX_CODE
#endif

		private readonly ILoadingCurtain _loadingCurtain;

		[Inject]
		public GameLoopState(
			ILoadingCurtain loadingCurtain,
			ILocalizationService localizationService,
			IPersistentProgressService persistentProgressService,
			IActiveRepository<IPresenter> presentersRepository
		)
		{
			_localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));

			_persistentProgressService =
				persistentProgressService ?? throw new ArgumentNullException(nameof(persistentProgressService));
			_presentersRepository = presentersRepository ?? throw new ArgumentNullException(nameof(presentersRepository));

			_loadingCurtain = loadingCurtain ?? throw new ArgumentNullException(nameof(loadingCurtain));
		}

		public UniTask Enter()
		{
			_loadingCurtain.HideSlowly();

			_localizationService.UpdateTranslations();

			EnablePresenters();

			_presentersRepository.Get<DissolveShaderViewController>().StartDissolving();
#if DEV
			SetMoreMoney();
#endif

			return UniTask.CompletedTask;
		}

		private void EnablePresenters()
		{
			_presentersRepository.EnableMany(
				new[]
				{
					typeof(UpgradeWindowPresenter)
				}
			);
		}

		public void Exit()
		{
			_loadingCurtain.Show();
			_presentersRepository.DisableAll();
		}

#if DEV
		private void SetMoreMoney()
		{
			_persistentProgressService.GlobalProgress.ResourceModel!.SetMoney(
				_persistentProgressService.GlobalProgress.ResourceModel.SoftCurrency.ReadOnlyMaxValue
			);
		}
#endif
	}
}
