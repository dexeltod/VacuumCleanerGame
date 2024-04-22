using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.DomainInterfaces
{
	public interface IResourceModelReadOnly
	{
		IResourceReadOnly<int> SoftCurrency { get; }
		IResourceReadOnly<int> TotalResourcesAmount { get; }
		IResourceReadOnly<int> Score { get; }
		IResourceReadOnly<int> HardCurrency { get; }

		int MaxCashScore { get; }
		int CurrentCashScore { get; }
		int CurrentTotalResources { get; }
		int PercentOfScore { get; }
		int MaxTotalResourceCount { get; }
	}
}