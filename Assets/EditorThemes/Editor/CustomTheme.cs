using System.Collections.Generic;
using UnityEngine;

namespace EditorThemes.Editor
{
	[System.Serializable] public class CustomTheme
	{
		public string Name;

		public enum UnityTheme
		{
			Dark,
			Light,
			Both
		}

		public UnityTheme unityTheme;
		public bool IsUnDeletable;
		public bool IsUnEditable;
		public string Version;

		public List<UIItem> Items;

		[System.Serializable] public class UIItem
		{
			public string Name;
			public Color Color;
		}
	}
}