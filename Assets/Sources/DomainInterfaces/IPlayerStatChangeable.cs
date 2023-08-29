using Sources.ServicesInterfaces;

namespace Sources.DomainInterfaces
{
	public interface IPlayerStatChangeable : IPlayerStat
	{
		void SetValue(int value);
	}
}