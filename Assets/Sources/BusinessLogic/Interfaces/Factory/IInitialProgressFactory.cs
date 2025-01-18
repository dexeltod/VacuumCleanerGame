using Sources.DomainInterfaces;

namespace Sources.BuisenessLogic.Interfaces.Factory
{
	public interface IInitialProgressFactory
	{
		IGlobalProgress Create();
	}
}