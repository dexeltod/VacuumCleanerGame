using UnityEngine;

namespace Sources.Application.StateMachine.GameStates
{
	public class UnityAuthorization : ServicesInterfaces.Authorization.IAuthorization
	{
		public void Authorize()
		{
			Debug.Log("UnityAuthorization");
		}
	}
}