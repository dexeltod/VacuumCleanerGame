using System;

namespace Sources.Infrastructure.StateMachine.GameStates
{
	public sealed class PlayerAccount : IPlayerAccount
	{
		public string Name { get; }
		public string UniqueId { get; }
		public string Language { get; }
		public string ProfilePicture { get; }

		public PlayerAccount(string name, string uniqueId, string language, string profilePicture)
		{
			Name = name ?? throw new ArgumentNullException(nameof(name));
			UniqueId = uniqueId;
			Language = language ?? throw new ArgumentNullException(nameof(language));
			ProfilePicture = profilePicture ?? throw new ArgumentNullException(nameof(profilePicture));
		}
	}
}