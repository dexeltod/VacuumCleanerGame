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

		[Inject] private ILocalizationService _localization;
		[Inject] private IAssetProvider _pizda;

		private void Start()
		{
			TextMeshProUGUI textMeshPro = GetComponent<TextMeshProUGUI>() ??
				throw new NullReferenceException(nameof(textMeshPro));

			textMeshPro.text = _localization.GetTranslationText(
				string.IsNullOrEmpty(_phraseName) == false ? _phraseName : textMeshPro.text
			);

			if (textMeshPro.text == null)
				throw new Exception("No such phrase in localization: " + _phraseName);
		}
	}
}