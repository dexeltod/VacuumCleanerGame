using System;
using Sources.Controllers.Common;
using Sources.PresentationInterfaces;
using Sources.Utils;
using UnityEngine;

namespace Sources.Controllers
{
	[RequireComponent(typeof(Rigidbody))]
	public abstract class MonoPresenter : MonoBehaviour
	{
		private Transformable _transformable;
		private Animator _animator;

		private IUpdatable _updatable = null;
		private bool _isMove = true;
		private AnimationHasher _animationHasher;
		private ParticleSystem _particleSystem;

		public void Initialize(
			Transformable model,
			Animator animator,
			AnimationHasher animationHasher
		)
		{
			_animationHasher = animationHasher ?? throw new ArgumentNullException(nameof(animationHasher));
			_transformable = model ?? throw new ArgumentNullException(nameof(model));
			_animator = animator ? animator : throw new ArgumentNullException(nameof(animator));

			if (_transformable is IUpdatable updatable)
				_updatable = updatable;

			enabled = true;

			OnMoved(model.Transform.position);
		}

		private void OnEnable()
		{
			_transformable.Looked += OnLookAt;
			_transformable.Moved += OnMoved;
			_transformable.Destroying += OnDestroying;
		}

		protected void DestroyCompose() =>
			_transformable.Destroy();

		private void OnDisable()
		{
			_transformable.Looked -= OnLookAt;
			_transformable.Moved -= OnMoved;
			_transformable.Destroying -= OnDestroying;
		}

		private void Update()
		{
			_updatable?.Update(Time.deltaTime);
			_animator.Play(_isMove ? _animationHasher.Run : _animationHasher.Idle);
		}

		private void OnLookAt(Vector3 direction)
		{
			if (direction != Vector3.zero)
				transform.rotation = Quaternion.LookRotation(direction);

			_isMove = direction != Vector3.zero;
		}

		private void OnMoved(Vector3 newPosition) =>
			transform.position = newPosition;

		private void OnDestroying() =>
			Destroy(gameObject);
	}
}
