namespace Sources.DomainInterfaces
{
	public interface IResourceModel : IResourceModelReadOnly
	{
		void AddMaxTotalResourceModifier(int newAmount);
		void AddMoney(int newValue);
		void AddScore(int newCashScore);
		void ClearAllScores();
		void ClearTotalResources();
		void DecreaseCashScore(int newValue);
		bool TryDecreaseMoney(int newValue);
	}
}