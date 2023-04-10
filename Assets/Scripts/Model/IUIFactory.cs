using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Model
{
	public interface IUIFactory : IService
	{
		Task<GameObject> CreateUI();
		event Action UICreated;
	}
}