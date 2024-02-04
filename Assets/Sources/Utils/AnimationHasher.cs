using UnityEngine;

namespace Sources.Utils
{
	public class AnimationHasher
	{
		public int Run => Animator.StringToHash("Run");
		public int Idle => Animator.StringToHash("Idle");
	}
}