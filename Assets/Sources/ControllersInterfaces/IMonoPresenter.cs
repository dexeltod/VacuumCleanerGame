using Sources.Utils;
using UnityEngine;

namespace Sources.ControllersInterfaces
{
	public interface IMonoPresenter
	{
		GameObject GameObject { get; }

		void Initialize(
			ITransformable model,
			Animator animator,
			AnimationHasher animationHasher
		);
	}
}