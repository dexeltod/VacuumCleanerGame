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

		public LocalizationService()
		{
			IResourceProvider resourceProvider = GameServices.Container.Get<IResourceProvider>();

			LeanLocalization leanLocalization = LoadAssets(resourceProvider, out var localizationData);

			AddLanguages(localizationData, leanLocalization);
			CreatePhrases(localizationData, leanLocalization);

			_phraseNames = new string[localizationData.Phrases.Count];

			for (int i = 0; i < localizationData.Phrases.Count; i++)
			{
				Phrase phrase = localizationData.Phrases[i];
				_phraseNames[i] = phrase.Name;
			}
		}

		private LeanLocalization LoadAssets(IResourceProvider resourceProvider, out LocalizationRoot localizationData)
		{
			LeanLocalization leanLocalization =
				resourceProvider.Load<LeanLocalization>(ResourcesAssetPath.GameObjects.LeanLocalization);

			localizationData = JsonUtility.FromJson<LocalizationRoot>(ResourcesAssetPath.Configs.Localization);
			return leanLocalization;
		}

		public string GetTranslationText(string phrase) =>
			LeanLocalization.GetTranslationText(_phraseNames.FirstOrDefault(elem => elem == phrase));

		private void AddLanguages(LocalizationRoot localizationData, LeanLocalization leanLocalization)
		{
			foreach (string language in localizationData.Languages)
				leanLocalization.AddLanguage(language);
		}

		private void CreatePhrases(LocalizationRoot localizationData, LeanLocalization leanLocalization)
		{
			for (var i = 0; i < localizationData.Phrases.Count; i++)
			{
				LeanPhrase phrase = leanLocalization.AddPhrase(localizationData.Phrases[i].Name);

				phrase.AddEntry(
					localizationData.Phrases[i].Translations[i].Language,
					localizationData.Phrases[i].Translations[i].Text
				);
			}
		}
	}
}