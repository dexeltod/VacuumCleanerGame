using System;
using System.Linq;
using Lean.Localization;
using Sources.Application.Utils.Configs;
using Sources.DIService;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Services.Localization
{
	public class LocalizationService : ILocalizationService
	{
		private readonly string[] _phraseNames;
		private readonly string[] _languages;

		public LocalizationService()
		{
			IResourceProvider resourceProvider = GameServices.Container.Get<IResourceProvider>();

			LeanLocalization leanLocalization = LoadAssets(resourceProvider, out var localizationData);

			_languages = new string[localizationData.Languages.Count];
			_phraseNames = new string[localizationData.Phrases.Count];

			AddLanguages(localizationData, leanLocalization);
			CreatePhrases(localizationData, leanLocalization);

			LeanLocalization.SetCurrentLanguageAll("Russian");
			LeanLocalization.UpdateTranslations();
		}

		public string GetTranslationText(string phrase)
		{
			string phraseHuy = _phraseNames.FirstOrDefault(elem => elem == phrase);

			string huy = LeanLocalization.GetTranslationText(phraseHuy);
			return huy;
		}

		public void SetLocalLanguage(string language)
		{
			if (_languages.Contains(language) == false)
				throw new InvalidOperationException($"Language {language} is not existing");

			LeanLocalization.SetCurrentLanguageAll(language);
		}

		private LeanLocalization LoadAssets(IResourceProvider resourceProvider, out LocalizationRoot localizationData)
		{
			LeanLocalization leanLocalization =
				resourceProvider.InstantiateAndGetComponent<LeanLocalization>(ResourcesAssetPath.GameObjects
					.LeanLocalization);

			string config = resourceProvider.Load<TextAsset>(ResourcesAssetPath.Configs.Localization).text;

			localizationData = JsonUtility.FromJson<LocalizationRoot>(config);
			return leanLocalization;
		}

		private void AddLanguages(LocalizationRoot localizationData, LeanLocalization leanLocalization)
		{
			for (int i = 0; i < localizationData.Languages.Count; i++)
			{
				string language = localizationData.Languages[i];
				_languages[i] = language;
				leanLocalization.AddLanguage(language);
			}
		}

		private void CreatePhrases(LocalizationRoot localizationData, LeanLocalization leanLocalization)
		{
			for (var x = 0; x < localizationData.Phrases.Count; x++)
			{
				LeanPhrase phrase = leanLocalization.AddPhrase(localizationData.Phrases[x].Name);

				for (int i = 0; i < _languages.Length; i++)
				{
					Phrase phraseData = localizationData.Phrases[x];

					phrase.AddEntry(
						phraseData.Translations[i].Language,
						phraseData.Translations[i].Text
					);
				}
			}

			InitPhraseNames(localizationData);
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