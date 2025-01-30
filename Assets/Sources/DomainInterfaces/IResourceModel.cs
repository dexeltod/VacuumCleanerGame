namespace Sources.DomainInterfaces
{
	public interface IResourceModel : IResourceModelReadOnly
	{
		void AddMaxTotalResourceModifier(int newAmount);
		void AddMoney(int newValue);
		void ClearAllScores();
		void ClearTotalResources();
		void DecreaseCashScore(int newValue);
		void SetMoney(int value);
		bool TryAddScore(int newCashScore);
		bool TryDecreaseMoney(int newValue);
	}
}