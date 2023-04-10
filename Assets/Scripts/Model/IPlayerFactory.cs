using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Model
{
	public interface IPlayerFactory : IService
	{
		GameObject MainCharacter { get; }
		event Action MainCharacterCreated;
		UniTask InstantiateHero(GameObject initialPoint);
	}
}