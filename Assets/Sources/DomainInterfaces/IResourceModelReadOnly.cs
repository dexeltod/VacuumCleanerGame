using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.DomainInterfaces
{
	public interface IResourceModelReadOnly
	{
		IReadOnlyProgressValue<int> SoftCurrency { get; }
		IReadOnlyProgressValue<int> TotalAmount { get; }
		IReadOnlyProgressValue<int> CashScore { get; }
		IReadOnlyProgressValue<int> HardCurrency { get; }

		int CurrentCashScore { get; }
		int CurrentTotalResources { get; }
		int MaxTotalResourceCount { get; }
	}
}