namespace Sources.ControllersInterfaces
{
	public interface IGameplayInterfacePresenter : IPresenter
	{
		public void OnGoToNextLevel();
		public void OnIncreaseSpeed();

		public void SetTotalResourceCount(int globalScore);
	}
}