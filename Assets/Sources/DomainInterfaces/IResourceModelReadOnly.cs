using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.DomainInterfaces
{
	public interface IResourceModelReadOnly
	{
		IReadOnlyProgress<int> SoftCurrency { get; }
		IReadOnlyProgress<int> TotalAmount { get; }
		IReadOnlyProgress<int> CashScore { get; }
		IReadOnlyProgress<int> HardCurrency { get; }

		int CurrentCashScore { get; }
		int CurrentTotalResources { get; }
		int MaxTotalResourceCount { get; }
	}
}