namespace Sources.PresentationInterfaces
{
	public interface ILoadingCurtain
	{
		void Hide();
		void HideSlowly();
		void SetText(string empty);
		void Show();
	}
}