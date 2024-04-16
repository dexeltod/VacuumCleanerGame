namespace Sources.DomainInterfaces
{
	public interface IGlobalProgress
	{
		IGameProgress UpgradeProgressModel { get; }
		ILevelProgress LevelProgress { get; }
		IGameProgress PlayerProgress { get; }
		IResourcesModel ResourcesModel { get; }
	}
}