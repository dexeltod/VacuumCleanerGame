namespace Sources.DIService
{
	public class GameServices
	{
		private static GameServices _instance;
		public static GameServices Container => _instance ??= new GameServices();

		public TService Register<TService>(TService implementation) where TService : IService => 
			Implementation<TService>.ServiceInstance = implementation;

		public TService Get<TService>() where TService : IService => 
			Implementation<TService>.ServiceInstance;

		private static class Implementation<TService> where TService : IService
		{
			public static TService ServiceInstance;
		}
	}
}