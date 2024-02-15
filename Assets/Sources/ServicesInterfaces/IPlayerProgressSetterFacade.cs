namespace Sources.ServicesInterfaces.DTO
{
	public interface IPlayerProgressSetterFacade
	{
		bool TryAddOneProgressPoint(string progressName);
	}
}