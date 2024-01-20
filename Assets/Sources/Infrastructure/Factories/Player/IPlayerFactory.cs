using System;
using Joystick_Pack.Scripts.Base;

using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.Factories.Player
{
	public interface IPlayerFactory 
	{
		GameObject Player { get; }

		GameObject Create(
			GameObject initialPoint,
			Joystick joystick,
			IPlayerStatsService stats,
			Action onErrorCallback = null
		);
	}
}