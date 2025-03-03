using Sources.DomainInterfaces;

namespace Sources.BusinessLogic.Services
{
	public interface IProgressCleaner
	{
		IGlobalProgress CreateClear();
	}
}