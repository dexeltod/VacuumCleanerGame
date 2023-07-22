using Sources.Core.Domain.Progress;

namespace Sources.Core.Domain.DomainInterfaces.DomainServicesInterfaces
{
	public interface IDataSaveLoader
	{
		void Save(object data);
		void DeleteSaves();
		GameProgressModel LoadProgress();
		void SetUniqueSaveFilePath();
	}
}