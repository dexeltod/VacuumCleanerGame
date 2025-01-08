using VContainer;
using VContainer.Unity;

namespace Sources.Boot.Bootstrapp
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
			new ServiceRegister(builder).Register();

		}
	}
}