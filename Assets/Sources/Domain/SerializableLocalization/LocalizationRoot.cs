using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Domain.SerializableLocalization
{
	[Serializable]
	public class LocalizationRoot
	{
		[SerializeField] public List<Phrase> Phrases;
		[SerializeField] public List<string> Languages;
	}
}