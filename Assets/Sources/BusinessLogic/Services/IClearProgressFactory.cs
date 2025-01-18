using Sources.DomainInterfaces;

namespace Sources.BusinessLogic.Services
{
	public interface IClearProgressFactory
	{
		IGlobalProgress Create();
	}
}
