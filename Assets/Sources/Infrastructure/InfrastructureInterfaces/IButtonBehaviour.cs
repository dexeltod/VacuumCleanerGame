using System;

namespace Sources.Infrastructure.InfrastructureInterfaces
{
	interface IButtonBehaviour : IMonoBehaviour
	{
		event Action ButtonPressed;
	}
}