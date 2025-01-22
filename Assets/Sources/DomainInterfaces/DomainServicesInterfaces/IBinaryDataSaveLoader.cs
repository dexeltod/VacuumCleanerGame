namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface IBinaryDataSaveLoader
	{
		void DeleteSaves();
		IGlobalProgress LoadProgress();
		void Save(object data);
		void SetUniqueSaveFilePath();
	}
}