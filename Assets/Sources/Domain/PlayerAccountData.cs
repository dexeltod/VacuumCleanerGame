using System;
using Sources.DomainInterfaces;

namespace Sources.Domain
{
	public sealed class PlayerAccountData : IPlayerAccount
	{
		public PlayerAccountData(string name, string uniqueId, string language, string profilePicture)
		{
			Name = name ?? throw new ArgumentNullException(nameof(name));
			UniqueId = uniqueId;
			Language = language ?? throw new ArgumentNullException(nameof(language));
			ProfilePicture = profilePicture ?? throw new ArgumentNullException(nameof(profilePicture));
		}

		public string Name { get; }
		public string UniqueId { get; }
		public string Language { get; }
		public string ProfilePicture { get; }
	}
}