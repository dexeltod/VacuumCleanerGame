using VContainer;
using VContainer.Unity;

namespace Sources.Application.Bootstrapp
{
	// ▄▄▄▄    ▒█████   ▒█████  ▄▄▄█████▓
	// ▓█████▄ ▒██▒  ██▒▒██▒  ██▒▓  ██▒ ▓▒
	// ▒██▒ ▄██▒██░  ██▒▒██░  ██▒▒ ▓██░ ▒░
	// ▒██░█▀  ▒██   ██░▒██   ██░░ ▓██▓ ░ 
	// ░▓█  ▀█▓░ ████▓▒░░ ████▓▒░  ▒██▒ ░ 
	// ░▒▓███▀▒░ ▒░▒░▒░ ░ ▒░▒░▒░   ▒ ░░   
	// ▒░▒   ░   ░ ▒ ▒░   ░ ▒ ▒░     ░    
	// ░    ░ ░ ░ ░ ▒  ░ ░ ░ ▒    ░      
	// ░          ░ ░      ░ ░           
	// ░                            
	public class Boot : LifetimeScope
	{
		protected override void Configure(IContainerBuilder builder)
		{
			base.Configure(builder);
			new ServiceRegister(builder).Register();
		}
	}
}