using System;

namespace Sources.ServicesInterfaces
{
	public interface IResourceProgressEventHandler
	{
		event Action<int> CashScoreChanged;
	}
}