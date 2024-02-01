using System;
using Sources.DomainInterfaces;
using Sources.Infrastructure.Presenters;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using Sources.UseCases.Scene;

namespace Sources.Application.StateMachine.GameStates
{
	public class SandContainerPresenterFactory
	{
		private readonly IResourcesModel _resourcesModel;
		private readonly ISandContainerView _sandContainerView;
		private readonly IResourceProgressEventHandler _resourceProgressEventHandler;
		private readonly ISandParticleSystem _sandParticle;
		private readonly ICoroutineRunner _coroutineRunner;

		public SandContainerPresenterFactory(
			IResourcesModel resourcesModel,
			ISandContainerView sandContainerView,
			IResourceProgressEventHandler resourceProgressEventHandler,
			ISandParticleSystem sandParticle,
			ICoroutineRunner coroutineRunner
		)
		{
			_resourcesModel = resourcesModel ?? throw new ArgumentNullException(nameof(resourcesModel));
			_sandContainerView = sandContainerView ?? throw new ArgumentNullException(nameof(sandContainerView));
			_resourceProgressEventHandler = resourceProgressEventHandler ??
				throw new ArgumentNullException(nameof(resourceProgressEventHandler));
			_sandParticle = sandParticle ?? throw new ArgumentNullException(nameof(sandParticle));
			_coroutineRunner = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));
		}

		public SandContainerPresenter Create(
		) =>
			new(
				_resourcesModel,
				_sandContainerView,
				_resourceProgressEventHandler,
				_sandParticle,
				_coroutineRunner
			);
	}
}