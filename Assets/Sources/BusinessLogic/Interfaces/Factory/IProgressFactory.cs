using System.Threading.Tasks;
using Sources.DomainInterfaces;

namespace Sources.BuisenessLogic.Interfaces.Factory
{
	public interface IProgressFactory
	{
		Task<IGlobalProgress> Create();
	}
}