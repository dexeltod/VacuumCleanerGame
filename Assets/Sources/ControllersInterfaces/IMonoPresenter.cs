using Sources.Utils;
using UnityEngine;

namespace Sources.ControllersInterfaces
{
	public interface IMonoPresenter
	{
		void Initialize(
			ITransformable model,
			Animator animator,
			AnimationHasher animationHasher
		);

		GameObject GameObject { get; }
	}
}
