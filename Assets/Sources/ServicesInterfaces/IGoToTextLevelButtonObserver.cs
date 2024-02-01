using System;

namespace Sources.PresentationInterfaces
{
	public interface IGoToTextLevelButtonObserver
	{
		public event Action GoToTextLevelButtonClicked;
		public event Action ButtonDestroying;
	}
}