using System;

namespace Model
{
	[Serializable]
	public class PlayerProgressData
	{
		public readonly SerializableItemsData SerializableItemsData;

		public PlayerProgressData(SerializableItemsData playerItemsData)
		{
			SerializableItemsData = playerItemsData;
		}
	}
}