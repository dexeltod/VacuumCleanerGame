using System;

namespace Sources.PresentationInterfaces
{
	public interface IGoToTextLevelButtonSubscribable
	{
		public event Action GoToTextLevelButtonClicked;
		public event Action ButtonDestroying;
	}
}