using Model.DI;
using Model.Infrastructure.Services.Factories;
using UnityEngine;

namespace Presenter.SceneEntity
{
	public class UpgradeWindow : MonoBehaviour
	{
		private IUIGetter _gameplayInterface;
		private Vacuum _player;

		private void Awake()
		{
			_player = ServiceLocator.Container.GetSingle<IPlayerFactory>()
				.MainCharacter
				.GetComponent<Vacuum>();

			_gameplayInterface = ServiceLocator.Container.GetSingle<IUIGetter>();
		}

		private void OnEnable()
		{
			
			_gameplayInterface.Joystick.enabled = false;
			_player.SetMoveBool(false);
			_gameplayInterface.This.SetActive(false);
		}

		private void OnDisable()
		{
			_gameplayInterface.Joystick.enabled = true;
			_player.SetMoveBool(true);
			_gameplayInterface.This.SetActive(true);
		}
	}
}