namespace Sources.BusinessLogic.Interfaces.Common.Factories
{
	public interface IFactory<out T>
	{
		T Create();
	}
}