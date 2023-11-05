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
		int MaxModifier { get; }
		int CurrentCashScore { get; }
		int GlobalSandCount { get; }
		int PercentOfScore { get; }

		void SetCashScore(int newValue);
		void AddCashScore(int newValue);
		void DecreaseCashScore(int newValue);
		void AddMoney(int newValue);
		void DecreaseMoney(int newValue);
		void ClearScores();
	}
}