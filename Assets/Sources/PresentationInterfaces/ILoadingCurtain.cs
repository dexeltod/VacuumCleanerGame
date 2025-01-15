namespace Sources.PresentationInterfaces
{
	public interface ILoadingCurtain
	{
		void SetText(string empty);
		void Show();
		void Hide();
		void HideSlowly();
	}
}