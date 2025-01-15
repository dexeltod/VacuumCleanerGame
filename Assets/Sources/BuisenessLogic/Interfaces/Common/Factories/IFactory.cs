namespace Sources.BuisenessLogic.Interfaces.Common.Factories
{
	public interface IFactory<out T>
	{
		T Create();
	}
}