namespace Sources.BusinessLogic.ServicesInterfaces
{
	public interface ILocalizationService
	{
		string GetTranslationText(string phrase);
		void SetLocalLanguage(string language);
		void UpdateTranslations();
	}
}
