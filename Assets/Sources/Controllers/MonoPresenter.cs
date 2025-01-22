using System;
using Sources.ControllersInterfaces;
using Sources.PresentationInterfaces;
using Sources.Utils;
using UnityEngine;

namespace Sources.Controllers
{
	[RequireComponent(typeof(Rigidbody))]
	public abstract class MonoPresenter : MonoBehaviour, IMonoPresenter
	{
		private AnimationHasher _animationHasher;
		private Animator _animator;
		private bool _isMove = true;
		private ParticleSystem _particleSystem;
		private ITransformable _transformable;

		private IUpdatable _updatable;

		private void Update()
		{
			_updatable?.Update(Time.deltaTime);
			_animator.Play(_isMove ? _animationHasher.Run : _animationHasher.Idle);
		}

		private void OnEnable()
		{
			_transformable.Looked += OnLookAt;
			_transformable.Moved += OnMoved;
			_transformable.Destroying += OnDestroying;
		}

		private void OnDisable()
		{
			_transformable.Looked -= OnLookAt;
			_transformable.Moved -= OnMoved;
			_transformable.Destroying -= OnDestroying;
		}

		public GameObject GameObject => gameObject;

		public void Initialize(
			ITransformable model,
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

		protected void DestroyCompose() =>
			_transformable.Destroy();

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