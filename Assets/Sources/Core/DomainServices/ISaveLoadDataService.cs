using Sources.Core;
using Sources.Core.Domain.Progress;

namespace Sources.DomainServices
{
	public interface ISaveLoadDataService : IService
	{
		void SaveProgress();
		GameProgressModel LoadProgress();
		void SaveToJson(string fileName, object data);
		string LoadFromJson(string fileName);
		T LoadFromJson<T>(string fileName);
	}
}