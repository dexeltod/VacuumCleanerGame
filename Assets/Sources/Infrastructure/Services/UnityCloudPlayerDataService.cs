using Cysharp.Threading.Tasks;
using Sources.ApplicationServicesInterfaces;
using UnityEngine;
using Random = System.Random;

namespace Sources.Infrastructure.StateMachine.GameStates
{
	public class UnityCloudPlayerDataService : ICloudPlayerDataService
	{
		public UniTask<IPlayerAccount> GetPlayerAccount() =>
			new(null);

		public void SetStatusInitialized() { }

		public void Authorize() { }
		public bool IsAuthorized => CheckAuthorization();

		private bool CheckAuthorization()
		{
			return false;
		}
	}
}