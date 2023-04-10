using UnityEngine;

namespace Model
{
	public interface ICamera : IService
	{
		Camera Camera { get; }
	}
}