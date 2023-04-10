using System;
using System.Linq;

namespace Model
{
	[Serializable]
	public class SerializableItemsData
	{
		private string[] _itemTitles;
		private string[] _itemReferences;

		public SerializableItemsData(string[] itemTitles, string[] itemReferences)
		{
			_itemTitles = itemTitles;
			_itemReferences = itemReferences;
		}

		public string[] GetItemReferences() => 
			_itemReferences;

		public string[] GetItemTitles() => 
			_itemTitles.ToArray();
	}
}