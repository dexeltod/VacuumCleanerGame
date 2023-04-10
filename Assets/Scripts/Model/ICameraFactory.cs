using System.Threading.Tasks;
using UnityEngine;

namespace Model
{
	public interface ICameraFactory : ICamera
	{
		Task<GameObject> CreateCamera();
	}
}