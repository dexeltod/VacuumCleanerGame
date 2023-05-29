using System;
using Cysharp.Threading.Tasks;
using Model.Infrastructure.Data;
using UnityEngine;

namespace ViewModel.Infrastructure.Services.Factories.Player
{
	public interface IPlayerFactory : IService
	{
		GameObject MainCharacter { get; }
		event Action MainCharacterCreated;
		UniTask Instantiate(GameObject initialPoint, IPresenterFactory presenterFactory, Joystick joystick,
			GameProgressModel progress);
	}
}