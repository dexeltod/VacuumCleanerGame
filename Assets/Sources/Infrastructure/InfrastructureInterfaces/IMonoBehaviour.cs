using System;

namespace Sources.Infrastructure.InfrastructureInterfaces
{
	interface IMonoBehaviour
	{
		event Action ActiveChanged;
		event Action Destroyed;
		
	}
}