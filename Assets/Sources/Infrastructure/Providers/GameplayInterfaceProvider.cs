using Sources.Infrastructure.Common;
using Sources.PresentationInterfaces;

namespace Sources.Infrastructure.Providers
{
	public class GameplayInterfaceProvider : Provider<IGameplayInterfaceView>
	{
		public  IGameplayInterfaceView Instance { get; protected set; }

		public override void Register(IGameplayInterfaceView instance) =>
			Instance = instance;
	}
}