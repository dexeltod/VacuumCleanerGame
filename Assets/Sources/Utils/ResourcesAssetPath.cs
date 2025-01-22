using Sources.Utils.AssetPaths;

namespace Sources.Utils
{
	public static class ResourcesAssetPath
	{
		public readonly static SceneResources Scene = new();
		public readonly static GameObjects GameObjects = new();
		public readonly static Materials Materials = new();
		public readonly static Configs Configs = new();
		public readonly static SoundNames SoundNames = new();
	}
}