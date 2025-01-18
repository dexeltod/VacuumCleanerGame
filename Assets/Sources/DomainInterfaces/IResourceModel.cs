namespace Sources.DomainInterfaces
{
	public interface IResourceModel : IResourceModelReadOnly
	{
		void AddScore(int newCashScore);
		void AddMaxTotalResourceModifier(int newAmount);
		void DecreaseCashScore(int newValue);
		void AddMoney(int newValue);
		bool TryDecreaseMoney(int newValue);
		void ClearAllScores();
		void ClearTotalResources();
	}
}
