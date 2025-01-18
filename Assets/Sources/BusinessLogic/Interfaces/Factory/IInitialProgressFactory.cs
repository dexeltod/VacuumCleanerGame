using Sources.DomainInterfaces;

namespace Sources.BusinessLogic.Interfaces.Factory
{
	public interface IInitialProgressFactory
	{
		IGlobalProgress Create();
	}
}
