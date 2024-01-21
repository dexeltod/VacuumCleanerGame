using VContainer;
using VContainer.Unity;

namespace Sources.Application
{
	public class Boot : LifetimeScope
	{
		private Game _game;

		protected override async void Configure(IContainerBuilder builder)
		{
			base.Configure(builder);
			new ServiceRegister(builder).Register();
		}
		
	}
}