using System.Collections.Generic;

namespace Sources.Services.Localization
{
	public interface ITranslatorService
	{
		void Localize(ref string phrase);
		void Localize(string[] phrases);
		List<string> Localize(List<string> phrases);
	}
}