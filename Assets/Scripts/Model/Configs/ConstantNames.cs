using Model.Data;
using Model.MainMenu;

namespace Model.Configs
{
	public static class ConstantNames
	{
		public static readonly UiElementNames UIElementNames = new();

		public static readonly MusicNames MusicNames = new();
		public static readonly ProgressNames ProgressNames = new();

		public const string PlayerSpawnPointTag = "InitialPoint";
		public const string Player = "Player";
		public const string InitialScene = "Init";
		public const string MenuScene = "MainMenu";
		public const string Confiner = "Confiner";
	}
}