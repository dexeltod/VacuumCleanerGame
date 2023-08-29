using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Services.Localization.Serializable
{
	[Serializable]
	public class LocalizationRoot
	{
		[SerializeField] public List<Phrase> Phrases;
		[SerializeField] public List<string> Languages;
	}
}