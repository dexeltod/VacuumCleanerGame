using Sources.ControllersInterfaces;
using Sources.DomainInterfaces;
using Sources.Utils;
using UnityEngine;

namespace Sources.BusinessLogic
{
	public class PlayerBody : MonoBehaviour, IPlayer
	{
		public void Initialize(ITransformable model, Animator animator, AnimationHasher animationHasher)
		{
		}
	}
}
