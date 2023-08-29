using System;

namespace Sources.ServicesInterfaces
{
	public interface IPlayerStat
	{
		int Value { get; }
		string Name { get; }
		public event Action ValueChanged;
	}
}