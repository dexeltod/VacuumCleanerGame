namespace Sources.DomainInterfaces
{
	public interface IResourceModelModifiable
	{
		void AddScore(int newCashScore);
		void AddMaxTotalResourceModifier(int newAmount);
		void DecreaseCashScore(int newValue);
		void AddMoney(int newValue);
		void DecreaseMoney(int newValue);
		void ClearAllScores();
		void ClearTotalResources();
		void AddMaxCashScoreModifier(int newAmount);
	}
}