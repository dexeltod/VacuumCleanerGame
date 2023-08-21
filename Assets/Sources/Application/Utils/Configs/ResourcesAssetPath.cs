namespace Sources.Application.Utils.Configs
{
	public static class ResourcesAssetPath
	{
		public static readonly SceneResources Scene = new();
		public static readonly GameObjects GameObjects = new();
		public static readonly Configs Configs = new();
	}

	public class SceneResources
	{
		public readonly UIResources UI = new();

		public readonly string CinemachineVirtualCamera = "Game/Cameras/VirtualCamera";
		public readonly string MainCamera = "Game/Cameras/Main Camera";
		public readonly string Player = "Game/Player/Player";
	}

	public class GameObjects
	{
		public readonly string Game = "Game/SceneConfigs/Game";
		public readonly string LeanLocalization = "Localization/LeanLocalisation";
	}

	public class UIResources
	{
		public readonly string UI = "UI/GameplayInterface";
		public readonly string UpgradeWindow = "UI/Shop/ShopItemList/UpgradeCanvas";
		public readonly string UpgradeElement = "UI/Shop/ShopItemList/UpgradeElement/UpgradeElement";
	}

	public class Configs
	{
		public readonly string Localization = "Config/LocalizationPhrases";
		public readonly string ShopItems = "UI/Shop/ShopItems";
	}
}