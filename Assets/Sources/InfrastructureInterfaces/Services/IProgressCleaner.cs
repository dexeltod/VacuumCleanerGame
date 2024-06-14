using Sources.DomainInterfaces;

namespace Sources.InfrastructureInterfaces.Services
{
	public interface IProgressCleaner
	{
		IGlobalProgress Clear();
	}
}