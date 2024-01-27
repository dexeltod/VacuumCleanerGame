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
		[Inject] private IObjectResolver _container;
		[SerializeField] private string _phraseName;

		private ILocalizationService _localization;

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