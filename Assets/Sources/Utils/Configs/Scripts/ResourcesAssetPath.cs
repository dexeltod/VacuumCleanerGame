namespace Sources.Utils.Configs.Scripts
{
	public static class ResourcesAssetPath
	{
		public static readonly SceneResources Scene = new();
		public static readonly GameObjects GameObjects = new();
		public static readonly Configs Configs = new();
	}

	public class SceneResources
	{
		public readonly UIResources UIResources = new();

		public readonly string CinemachineVirtualCamera = "Game/Cameras/VirtualCamera";
		public readonly string MainCamera = "Game/Cameras/Main Camera";
		public readonly string Player = "Game/Player/Player";
	}

	public class GameObjects
	{
		public readonly string CoroutineRunner = "Game/Coroutine/CoroutineRunner";
		public readonly string ModifiableMesh = "Game/Environment/Sand/SandGround";
		public readonly string LeanLocalization = "Localization/LeanLocalization";
		public readonly string UpgradeTrigger = "Game/Environment/UpgradeTrigger";
	}

	public class EditorUi
	{
		public readonly string AuthHandler = "Game/Ui/EditorUi/AuthHandler";
	}

	public class Yandex
	{
		public readonly string AuthHandler = "Game/Yandex/AuthHandler";
	}

	public class UIResources
	{
		public readonly string ShopItems = "Game/UI/Shop/ShopItems";
		public readonly string LoadingCurtain = "Game/UI/LoadingCurtain";
		public readonly Yandex Yandex = new();
		public readonly EditorUi Editor = new();
		public readonly string UI = "Game/UI/GameplayInterface";
		public readonly string UpgradeWindow = "Game/UI/Shop/ShopItemList/UpgradeCanvas";
		public readonly string MainMenuCanvas = "Game/UI/MainMenu/MainMenuCanvas";
	}

	public class Configs
	{
		public readonly string Game = "Game/SceneConfigs/Game";
		public readonly string Localization = "Config/Localization";
		public readonly string ProgressItems = "Config/ProgressItems";
		public readonly string LevelsConfig = "Config/LevelsConfig";
	}
}