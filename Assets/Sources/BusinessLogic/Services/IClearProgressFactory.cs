using Sources.DomainInterfaces;

namespace Sources.BuisenessLogic.Services
{
	public interface IClearProgressFactory
	{
		IGlobalProgress Create();
	}
}
