using System;
using Sources.ApplicationServicesInterfaces.Authorization;
using UnityEngine;

namespace Sources.Infrastructure.Factories.LeaderBoard
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