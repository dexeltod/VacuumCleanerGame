using System;
using UnityEngine;

namespace Sources.Services.Localization
{
	[Serializable]
	public class Translation
	{
		[SerializeField] public string Language;
		[SerializeField] public string Text;
	}
}