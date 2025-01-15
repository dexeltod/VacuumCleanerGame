using System;
using UnityEngine;

namespace Sources.Domain.SerializableLocalization
{
	[Serializable]
	public class Translation
	{
		[SerializeField] public string Language;
		[SerializeField] public string Text;
	}
}