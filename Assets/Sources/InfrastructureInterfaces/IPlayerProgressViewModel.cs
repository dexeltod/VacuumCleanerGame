namespace InfrastructureInterfaces
{
	public interface IPlayerProgressViewModel : IService
	{
		void SetProgress(string progressName);
	}
}