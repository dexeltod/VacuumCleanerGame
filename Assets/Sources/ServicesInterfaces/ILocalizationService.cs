using Sources.DIService;

namespace Sources.ServicesInterfaces
{
	public interface ILocalizationService : IService
	{
		string GetTranslationText(string phrase);
		void SetLocalLanguage(string language);
		void UpdateTranslations();
	}
}