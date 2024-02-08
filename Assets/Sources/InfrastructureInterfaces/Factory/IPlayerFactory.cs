using System;
using Joystick_Pack.Scripts.Base;
using Sources.ServicesInterfaces;

namespace Sources.InfrastructureInterfaces.Factory
{
	public interface IPlayerFactory 
	{
		UnityEngine.GameObject Player { get; }

		UnityEngine.GameObject Create(
			UnityEngine.GameObject initialPoint,
			Joystick joystick,
			IPlayerStatsService stats,
			Action onErrorCallback = null
		);
	}
}