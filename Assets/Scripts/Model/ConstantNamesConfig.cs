using Model.Infrastructure.Data;
using Model.MainMenu;

namespace Model
{
	public static class ConstantNamesConfig
	{
		public static readonly MusicNames MusicNames = new();
		public static readonly ProgressNames ProgressNames = new();

		public const string PlayerSpawnPointTag = "InitialPoint";
		public const string Player = "Player";
		public const string FirstLevel = "Level_1";
		public const string InitialScene = "Init";
		public const string MenuScene = "MainMenu";
		public const string Confiner = "Confiner";
		public const string PlayerPrefabPath = "Prebafs/Game/Characters/MainCharacter/Hugo";
	}
}