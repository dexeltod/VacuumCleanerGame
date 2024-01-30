using System.Collections.Generic;

namespace Sources.Services.Localization
{
	public interface ITranslatorService
	{
		string Localize(string phrase);
		void Localize(string[] phrases);
		List<string> Localize(List<string> phrases);
	}
}