namespace Sources.DomainInterfaces
{
	public interface IUpgradeProgressData
	{
		string Name { get; }
		int Value { get; set; }
		int MaxPointLevel { get; }
	}
}