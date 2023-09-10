using System;
using Sources.DIService;
using Sources.ServicesInterfaces;
using TMPro;
using UnityEngine;

namespace UseCases.Scene
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class TmpLocalizationBehaviour : MonoBehaviour
	{
		[SerializeField] private string _phraseName;
		private ILocalizationService _localization;

		private void Start()
		{
			_localization = GameServices.Container.Get<ILocalizationService>();
			TextMeshProUGUI textMeshPro = GetComponent<TextMeshProUGUI>();

			if (string.IsNullOrEmpty(_phraseName) == false)
				textMeshPro.text = _localization.GetTranslationText(_phraseName);
			else
				textMeshPro.text = _localization.GetTranslationText(textMeshPro.text);

			if (textMeshPro.text == null)
				throw new Exception("Couldn't find translation");
		}
	}
}