using System;

namespace Sources.PresentationInterfaces
{
	public interface IGoToTextLevelButtonSubscribeable
	{
		public event Action GoToTextLevelButtonClicked;
	}
}