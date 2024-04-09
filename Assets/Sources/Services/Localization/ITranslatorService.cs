using System.Collections.Generic;

namespace Sources.Services.Localization
{
	public interface ITranslatorService
	{
		string GetLocalize(string phrase);
		void GetLocalize(string[] phrases);
		List<string> GetLocalize(List<string> phrases);
	}
}