using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Model.Infrastructure.Services.Factories
{
	public interface IUIFactory : IService
	{
		public Joystick Joystick { get; }
		Task<GameObject> CreateUI();
		event Action UICreated;
	}
}