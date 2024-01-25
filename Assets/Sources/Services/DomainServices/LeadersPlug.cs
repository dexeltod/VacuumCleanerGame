using System.Collections.Generic;

namespace Sources.Services.DomainServices
{
	public class LeadersPlug
	{
		private readonly Dictionary<string, int> _players;

		public LeadersPlug() =>
			_players = new Dictionary<string, int>()
			{
				{ "player1", 1 },
				{ "player2", 2 },
				{ "player3", 3 },
				{ "player4", 4 },
				{ "player5", 5 },
			};

		public Dictionary<string, int> GetTestLeaders() =>
			_players;
	}
}