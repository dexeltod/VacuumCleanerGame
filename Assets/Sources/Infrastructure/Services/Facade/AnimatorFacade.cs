using System.Collections;
using UnityEngine;

namespace Sources.Infrastructure.Services.Facade
{
	public class AnimatorFacade : MonoBehaviour
	{
		private Animator _animator;

		private int _currentAnimationHash;

		private Coroutine _currentAnimationRoutine;

		private void Start() => _animator = GetComponent<Animator>();

		public void Play(int hash)
		{
			StopAnimationRoutine();
			_currentAnimationRoutine = StartCoroutine(StartAnimationRoutine(hash));
		}

		private void StopAnimationRoutine()
		{
			if (_currentAnimationRoutine != null)
				StopCoroutine(_currentAnimationRoutine);
		}

		private IEnumerator StartAnimationRoutine(int hash)
		{
			var timeOut = 1f;

			while (GetAnimatorInfo().shortNameHash != hash)
			{
				_animator.Play(hash);
				_animator.Update(Time.deltaTime);
				timeOut -= Time.deltaTime;

				if (timeOut <= 0)
					yield break;

				yield return 0;
			}
		}

		private AnimatorStateInfo GetAnimatorInfo() => _animator.GetCurrentAnimatorStateInfo(0);
	}
}