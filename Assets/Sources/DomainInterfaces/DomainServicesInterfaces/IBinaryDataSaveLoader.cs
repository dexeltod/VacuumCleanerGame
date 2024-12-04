namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface IBinaryDataSaveLoader
	{
		void Save(object data);
		void DeleteSaves();
		IGlobalProgress LoadProgress();
		void SetUniqueSaveFilePath();
	}
}