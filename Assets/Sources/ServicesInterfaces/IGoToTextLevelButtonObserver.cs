using System;

namespace Sources.ServicesInterfaces
{
	public interface IGoToTextLevelButtonObserver
	{
		public event Action GoToTextLevelButtonClicked;
		public event Action ButtonDestroying;
	}
}