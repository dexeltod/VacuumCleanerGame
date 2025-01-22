using Sources.Boot.Scripts.DIRegisters;
using VContainer;
using VContainer.Unity;

namespace Sources.Boot.Scripts
{
	public class Boot : LifetimeScope
	{
		protected override void Configure(IContainerBuilder builder)
		{
			new ServiceRegister(builder).Register();
		}
	}
}