using System.Collections.Generic;
using System.Linq;
using Sources.ServicesInterfaces;
using TMPro;
using UnityEngine;

namespace Sources.Presentation.UI
{
	public class TmpPhrases : MonoBehaviour, ITmpPhrases
	{
		[SerializeField] private TextMeshProUGUI[] _phrases;

		public List<string> Phrases
		{
			get => _phrases.Select(phrase => phrase.text).ToList();
			set
			{
				for (int i = 0; i < _phrases.Length; i++)
					_phrases[i].SetText(value[i]);
			}
		}
	}
}