using Joystick_Pack.Scripts.Base;
using Sources.DIService;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.Factories.Player
{
	public interface IPlayerFactory : IService
	{
		GameObject Player { get; }
		void Instantiate(GameObject initialPoint, Joystick joystick,
			IPlayerStatsService stats);
	}
}