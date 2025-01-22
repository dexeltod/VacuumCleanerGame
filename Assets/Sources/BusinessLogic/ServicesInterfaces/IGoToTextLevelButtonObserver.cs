using System;

namespace Sources.BusinessLogic.ServicesInterfaces
{
	public interface IGoToTextLevelButtonObserver
	{
		public event Action GoToTextLevelButtonClicked;
		public event Action ButtonDestroying;
	}
}