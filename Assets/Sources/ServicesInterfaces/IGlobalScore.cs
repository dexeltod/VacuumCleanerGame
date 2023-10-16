using System;

namespace Sources.ServicesInterfaces
{
	public interface IGlobalScore
	{
		event Action GlobalScoreChanged;
		int          GlobalScore { get; }
		event Action HalfGlobalScoreReached;
	}
}