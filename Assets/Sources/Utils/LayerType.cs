using System;

namespace Sources.Utils
{
	public enum LayerType
	{
		Resource,
		Player,
		Bound
	}

	public static class LayerService
	{
		public static string GetNameByType(Enum layerType) => Enum.GetName(layerType.GetType(), layerType);
	}
}
