using System;

namespace Sources.DomainInterfaces
{
	public interface IPlayerStat
	{
		int                 Value { get; }
		string              Name  { get; }
		public event Action ValueChanged;
	}
}