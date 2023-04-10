namespace Model
{
	public interface IPersistentProgressService : IService
	{
		GameProgress GameProgress { get; set; }
	}
}