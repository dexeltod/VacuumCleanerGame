using UnityEngine;
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
			new ServiceRegister(builder).Register();

			Debug.Log("Finish Boot");
		}
	}
}