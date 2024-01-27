using System.Collections.Generic;

namespace Sources.Services.DomainServices
{
	public class LeadersPlug
	{
		private readonly Dictionary<string, int> _players;

		public LeadersPlug() =>
			_players = new Dictionary<string, int>()
			{
				{ "player1", 100 },
				{ "player2", 300 },
				{ "player3", 50 },
				{ "player4", 300},
				{ "player5", 200},
			};

		public Dictionary<string, int> GetTestLeaders() =>
			_players;
	}
}