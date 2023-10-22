using System;

namespace Sources.PresentationInterfaces
{
	public interface IGoToTextLevelButtonSubscribeable
	{
		public event Action GoToTextLevelButtonClicked;
		public event Action ButtonDestroying;
	}
}