using Sources.DIService;
using UnityEngine;

namespace Sources.InfrastructureInterfaces.Scene
{
	public interface ICamera : IService
	{
		Camera Camera { get; }
	}
}