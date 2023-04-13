namespace Model.Infrastructure.Services
{
	public interface ISceneLoad : ISceneLoadInformer
	{
		void InvokeSceneLoaded();
	}
}