using System;

namespace Sources.DomainInterfaces
{
	public interface IPlayerStatSubscribable
	{
		public event Action ValueChanged;
	}
}