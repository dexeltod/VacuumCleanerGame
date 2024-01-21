namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface IDataSaveLoader
	{
		void Save(object data);
		void DeleteSaves();
		IGameProgressModel LoadProgress();
		void SetUniqueSaveFilePath();
	}
}