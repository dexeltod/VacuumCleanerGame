using Sources.DomainInterfaces;

namespace Sources.Infrastructure.Factories.Domain
{
	public interface IInitialProgressFactory
	{
		IGlobalProgress Create();
	}
}