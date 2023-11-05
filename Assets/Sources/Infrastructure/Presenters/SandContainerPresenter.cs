using System;
using JetBrains.Annotations;
using Sources.DomainInterfaces;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;

namespace Sources.Infrastructure.Presenters
{
	public class SandContainerPresenter
	{
		private readonly IResourcesModel _resourcesModel;
		private readonly ISandContainerView _sandContainerView;
		private readonly IResourceProgressEventHandler _resourceProgressEventHandler;

		private int _maxCashPoints;
		private int _currentCashScore;

		public SandContainerPresenter
		(
			[NotNull] IResourcesModel resourcesModel,
			[NotNull] ISandContainerView sandContainerView
		)
		{
			_resourcesModel = resourcesModel ?? throw new ArgumentNullException(nameof(resourcesModel));
			_sandContainerView = sandContainerView ?? throw new ArgumentNullException(nameof(sandContainerView));

			_maxCashPoints = resourcesModel.MaxCashScore;
			_currentCashScore = resourcesModel.CurrentCashScore;
		}

		private void Recalculate() { }
	}
}