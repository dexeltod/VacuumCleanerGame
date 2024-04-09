using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.DomainInterfaces
{
	public interface IResourcesModel
	{
		IResourceReadOnly<int> SoftCurrency { get; }
		IResourceReadOnly<int> TotalResourcesAmount { get; }
		IResourceReadOnly<int> Score { get; }
		IResourceReadOnly<int> HardCurrency { get; }

		int MaxCashScore { get; }
		int CurrentCashScore { get; }
		int CurrentTotalResources { get; }
		int PercentOfScore { get; }
		int MaxGlobalScore { get; }

		void AddScore(int newCashScore);
		void DecreaseCashScore(int newValue);
		void AddMoney(int newValue);
		void DecreaseMoney(int newValue);
		void ClearScores();
	}
}