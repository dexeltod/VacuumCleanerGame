namespace Sources.Application.Utils.Configs
{
	public static class ResourcesAssetPath
	{
		public static readonly SceneResources Scene = new();
		public static readonly SceneConfigResources SceneConfigs = new();
		public static readonly ShopConfigResources ShopConfig = new();
	}

	public class SceneResources
	{
		public readonly UIResources UI = new();

		public readonly string CinemachineVirtualCamera = "Game/Cameras/VirtualCamera";
		public readonly string MainCamera = "Game/Cameras/Main Camera";
		public readonly string Player = "Game/Player/Player";
	}

	public class SceneConfigResources
	{
		public readonly string Game = "Game/SceneConfigs/Game";
	}

	public class UIResources
	{
		public readonly string UI = "UI/GameplayInterface";
		public readonly string UpgradeWindow = "UI/Shop/ShopItemList/UpgradeCanvas";
		public readonly string UpgradeElement = "UI/Shop/ShopItemList/UpgradeElement/UpgradeElement";
	}

	public class ShopConfigResources
	{
		public readonly string ShopItems = "UI/Shop/ShopItems";
	}
}