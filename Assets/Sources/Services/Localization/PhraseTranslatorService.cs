using System;
using System.Collections.Generic;
using System.Linq;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Services.Localization
{
	public class PhraseTranslatorService : ITranslatorService
	{
		private readonly ILocalizationService _localizationService;

		public PhraseTranslatorService(ILocalizationService localizationService) =>
			_localizationService = localizationService;

		public string Localize(string phrase)
		{
			if (string.IsNullOrWhiteSpace(phrase))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(phrase));

			return _localizationService.GetTranslationText(phrase);
		}

		public void Localize(string[] phrases)
		{
			string[] result = new string[phrases.Length];
			
			for (int i = 0; i < phrases.Length; i++)
			{
				if (string.IsNullOrWhiteSpace(phrases[i]))
					throw new ArgumentException("Value cannot be null or whitespace.", (phrases[i]));

				phrases[i] = _localizationService.GetTranslationText(phrases[i]);
			}
		}

		public List<string> Localize(List<string> phrases)
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