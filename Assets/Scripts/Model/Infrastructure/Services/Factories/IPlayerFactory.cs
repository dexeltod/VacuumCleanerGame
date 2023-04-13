using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Model.Infrastructure.Services.Factories
{
	public interface IPlayerFactory : IService
	{
		GameObject MainCharacter { get; }
		event Action MainCharacterCreated;
		UniTask Instantiate(GameObject initialPoint, IPresenterFactory presenterFactory, Joystick joystick);
	}
}