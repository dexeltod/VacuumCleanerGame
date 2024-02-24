using Sources.DomainInterfaces.DomainServicesInterfaces;

namespace Sources.DomainInterfaces
{
	public interface IResourcesModel
	{
		IResourceReadOnly<int> SoftCurrency { get; }
		IResourceReadOnly<int> GlobalScore { get; }
		IResourceReadOnly<int> Score { get; }
		IResourceReadOnly<int> HardCurrency { get; }

		int MaxCashScore { get; }
		int CurrentCashScore { get; }
		int CurrentGlobalScore { get; }
		int PercentOfScore { get; }
		int MaxGlobalScore { get; }

		void AddScore(int newScore);
		void DecreaseCashScore(int newValue);
		void AddMoney(int newValue);
		void DecreaseMoney(int newValue);
		void ClearScores();
	}
}