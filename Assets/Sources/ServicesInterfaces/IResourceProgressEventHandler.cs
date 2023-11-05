using System;
using Sources.DIService;

namespace Sources.ServicesInterfaces
{
	public interface IResourceProgressEventHandler
	{
		event Action<int> SoftCurrencyChanged;
		event Action<int> CashScoreChanged;
		event Action<int> GlobalScoreChanged;
		event Action<int> MaxGlobalScoreChanged;
		event Action<int> MaxCashScoreChanged;
		event Action<bool> HalfGlobalScoreReached;
	}
}