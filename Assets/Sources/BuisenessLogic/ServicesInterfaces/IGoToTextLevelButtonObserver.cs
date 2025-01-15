using System;

namespace Sources.BuisenessLogic.ServicesInterfaces
{
	public interface IGoToTextLevelButtonObserver
	{
		public event Action GoToTextLevelButtonClicked;
		public event Action ButtonDestroying;
	}
}