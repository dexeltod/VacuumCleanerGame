using Sources.Core;
using UnityEngine;

namespace Sources.Infrastructure.InfrastructureInterfaces.Scene
{
	public interface ICamera : IService
	{
		Camera Camera { get; }
	}
}