using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Domain.SerializableLocalization
{
	[Serializable]
	public class Phrase
	{
		[SerializeField] public string Name;
		[SerializeField] public List<Translation> Translations;
	}
}
