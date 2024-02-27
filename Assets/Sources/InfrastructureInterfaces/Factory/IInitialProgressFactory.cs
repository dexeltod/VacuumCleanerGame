using Sources.DomainInterfaces;

namespace Sources.InfrastructureInterfaces.Factory
{
	public interface IInitialProgressFactory
	{
		IGlobalProgress Create();
	}
}