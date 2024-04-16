using Sources.InfrastructureInterfaces;

namespace Sources.Infrastructure.Services
{
	public class PlayerStatsNames : IPlayerStatsNames
	{
		private readonly string _speed = "Speed";
		private readonly string _scoreCash = "ScoreCash";
		private readonly string _maxScoreCash = "MaxScoreCash";
		public string Speed => _speed;
		public string ScoreCash => _scoreCash;
		public string MaxScoreCash => _maxScoreCash;
	}
}