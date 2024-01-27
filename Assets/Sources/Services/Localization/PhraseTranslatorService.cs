using System;
using System.Collections.Generic;
using System.Linq;
using Sources.ServicesInterfaces;

namespace Sources.Services.Localization
{
	public class PhraseTranslatorService : ITranslatorService
	{
		private readonly ILocalizationService _localizationService;

		public PhraseTranslatorService(ILocalizationService localizationService) =>
			_localizationService = localizationService;

		public void Localize(ref string phrase)
		{
			if (string.IsNullOrWhiteSpace(phrase))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(phrase));

			phrase = _localizationService.GetTranslationText(phrase);
		}

		public void Localize(string[] phrases)
		{
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
					throw new ArgumentException("Value cannot be null or whitespace.", (phrases[i]));

				var a  = _localizationService.GetTranslationText(phrases[i]);
				phrases[i] = a;
			}

			return phrases;
		}
	}
}