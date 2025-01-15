using Sources.DomainInterfaces;

namespace Sources.BuisenessLogic.Services
{
	public interface IProgressCleaner
	{
		IGlobalProgress Clear();
	}
}