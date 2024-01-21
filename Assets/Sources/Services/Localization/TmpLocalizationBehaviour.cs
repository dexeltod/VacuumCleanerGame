using System;
using Sources.ServicesInterfaces;
using TMPro;
using UnityEngine;
using VContainer;

namespace Sources.Services.Localization
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class TmpLocalizationBehaviour : MonoBehaviour
	{
		[SerializeField] private string _phraseName;
		private ILocalizationService _localization;
		
		[Inject]
		private void Construct(IObjectResolver container)
		{
			_localization = container.Resolve<ILocalizationService>();
		}

		private void Start()
		{
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