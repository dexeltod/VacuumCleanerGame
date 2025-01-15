using UnityEngine.UIElements;

namespace Plugins.SerializeInterfaces.Editor.UIElements
{
	internal class Tab : Toggle
	{
		public Tab(string text) : base()
		{
			base.text = text;
			RemoveFromClassList(
				ussClassName
			);
			AddToClassList(
				ussClassName
			);
		}
	}
}