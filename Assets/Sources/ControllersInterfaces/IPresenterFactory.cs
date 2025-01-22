namespace Sources.ControllersInterfaces
{
	public interface IPresenterFactory<out T> where T : class, IPresenter
	{
	}
}