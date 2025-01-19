using System;
using System.Linq;
using Plugins.CW.LeanLocalization.Required.Scripts;
using Sources.BusinessLogic.ServicesInterfaces;
using Sources.Domain.SerializableLocalization;
using Sources.Utils;
using UnityEngine;

namespace Sources.BusinessLogic
{
	public class LocalizationService : ILocalizationService
	{
		private const string StartLanguage = "ru";

		private readonly IAssetFactory _factory;
		private readonly string[] _phraseNames;
		private readonly string[] _languages;

		public LocalizationService(IAssetFactory factory)
		{
			_factory = factory ?? throw new ArgumentNullException(nameof(factory));

			LeanLocalization leanLocalization = LoadAssets(factory, out var localizationData);

			_languages = new string[localizationData.Languages.Count];
			_phraseNames = new string[localizationData.Phrases.Count];

			AddLanguages(localizationData, leanLocalization);
			CreatePhrases(localizationData, leanLocalization);

#if YANDEX_CODE
			return;
#endif

			LeanLocalization.SetCurrentLanguageAll(
				StartLanguage
			);
			LeanLocalization.UpdateTranslations();
		}

		public void UpdateTranslations() =>
			LeanLocalization.UpdateTranslations();

		public string GetTranslationText(string phrase)
		{
			string parsedPhrase = _phraseNames.FirstOrDefault(
				elem =>
				{
					if (string.IsNullOrWhiteSpace(
						    elem
					    ))
						throw new ArgumentException(
							"Value cannot be null or whitespace.",
							(elem)
						);

					return elem == phrase;
				}
			);

			return LeanLocalization.GetTranslationText(
				parsedPhrase
			);
		}

		public void SetLocalLanguage(string language)
		{
			if (_languages.Contains(
				    language
			    ) ==
			    false)
				throw new InvalidOperationException(
					$"Language {language} is not existing"
				);

			LeanLocalization.SetCurrentLanguageAll(
				language
			);
		}

		private LeanLocalization LoadAssets(IAssetFactory assetFactory, out LocalizationRoot localizationData)
		{
			LeanLocalization leanLocalization =
				assetFactory.InstantiateAndGetComponent<LeanLocalization>(
					ResourcesAssetPath.GameObjects.LeanLocalization
				);

			var config = Resources.Load<TextAsset>(
				ResourcesAssetPath.Configs.Localization
			);

			localizationData = JsonUtility.FromJson<LocalizationRoot>(
				config.text
			);
			return leanLocalization;
		}

		private void AddLanguages(LocalizationRoot localizationData, LeanLocalization leanLocalization)
		{
			for (int i = 0; i < localizationData.Languages.Count; i++)
			{
				string language = localizationData.Languages[i];
				_languages[i] = language;
				leanLocalization.AddLanguage(
					language
				);
			}
		}

		private void CreatePhrases(LocalizationRoot localizationData, LeanLocalization leanLocalization)
		{
			for (var x = 0; x < localizationData.Phrases.Count; x++)
			{
				LeanPhrase phrase = leanLocalization.AddPhrase(
					localizationData.Phrases[x].Name
				);

				for (int i = 0; i < _languages.Length; i++)
				{
					Phrase phraseData = localizationData.Phrases[x];

					phrase.AddEntry(
						phraseData.Translations[i].Language,
						phraseData.Translations[i].Text
					);
				}
			}

			InitPhraseNames(
				localizationData
			);
		}

		private void InitPhraseNames(LocalizationRoot localizationData)
		{
			for (int i = 0; i < localizationData.Phrases.Count; i++)
			{
				Phrase phrase = localizationData.Phrases[i];
				_phraseNames[i] = phrase.Name;
			}
		}
	}
}
