using UnityEngine;

namespace Sources.Infrastructure.Factories.Player
{
	public class AnimationHasher
	{
		public int Run => Animator.StringToHash("Run");
		public int Idle => Animator.StringToHash("Idle");
	}
}