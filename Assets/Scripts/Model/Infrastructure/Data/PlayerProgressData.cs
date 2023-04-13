using System;

namespace Model.Infrastructure.Data
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