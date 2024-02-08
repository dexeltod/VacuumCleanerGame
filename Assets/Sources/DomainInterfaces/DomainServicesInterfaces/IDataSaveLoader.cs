namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface IDataSaveLoader
	{
		void Save(object data);
		void DeleteSaves();
		IGameProgressProvider LoadProgress();
		void SetUniqueSaveFilePath();
	}
}