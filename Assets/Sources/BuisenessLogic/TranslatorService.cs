using System;
using System.Collections.Generic;
using Sources.BuisenessLogic.ServicesInterfaces;
using UnityEngine;

namespace Sources.BuisenessLogic
{
	public class TranslatorService
	{
		private readonly ILocalizationService _localizationService;

		public TranslatorService(ILocalizationService localizationService) =>
			_localizationService = localizationService;

		public string GetLocalize(string phrase)
		{
			if (string.IsNullOrWhiteSpace(phrase))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(phrase));

			return _localizationService.GetTranslationText(phrase);
		}

		public void GetLocalize(string[] phrases)
		{
			for (int i = 0; i < phrases.Length; i++)
			{
				if (string.IsNullOrWhiteSpace(phrases[i]))
					throw new ArgumentException("Value cannot be null or whitespace.", (phrases[i]));

				phrases[i] = _localizationService.GetTranslationText(phrases[i]);
			}
		}

		public List<string> GetLocalize(List<string> phrases)
		{
			for (int i = 0; i < phrases.Count; i++)
			{
				if (string.IsNullOrWhiteSpace(phrases[i]))
					throw new ArgumentException($"Value cannot be null or whitespace. Phrase {(phrases[i])}");

				var translatedText = _localizationService.GetTranslationText(phrases[i]);

				if (string.IsNullOrWhiteSpace(translatedText))
					Debug.LogAssertion($"not found phrase {phrases[i]}");

				phrases[i] = translatedText;
			}

			return phrases;
		}
	}
}