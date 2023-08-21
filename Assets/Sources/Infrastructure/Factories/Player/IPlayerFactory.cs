using Cysharp.Threading.Tasks;
using Joystick_Pack.Scripts.Base;
using Sources.DIService;
using Sources.InfrastructureInterfaces;
using Sources.InfrastructureInterfaces.Factory;
using Sources.Services.Interfaces;
using Sources.ServicesInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.Factories.Player
{
	public interface IPlayerFactory : IService
	{
		GameObject Player { get; }
		void Instantiate(GameObject initialPoint, IPresenterFactory presenterFactory, Joystick joystick,
			IPlayerStatsService stats);
	}
}