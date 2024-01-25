using System;
using Sources.ServicesInterfaces.Authorization;
using UnityEngine;

namespace Sources.Application.StateMachine.GameStates
{
	public class EditorAuthorization : MonoBehaviour, IAuthorization
	{
		public event Action<bool> AuthorizeCallback;

		public void EnableAuthorizeWindow()
		{
			throw new NotImplementedException();
		}

		public void DisableAuthorizeWindow() =>
			throw new NotImplementedException();
	}
}