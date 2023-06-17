using System;
using Cysharp.Threading.Tasks;
using Model.Data;
using UnityEngine;

namespace ViewModel.Infrastructure.Services.Factories.Player
{
	public interface IPlayerFactory : IService
	{
		GameObject Player { get; }
		event Action PlayerCreated;
		UniTask Instantiate(GameObject initialPoint, IPresenterFactory presenterFactory, Joystick joystick,
			GameProgressModel progress);
	}
}