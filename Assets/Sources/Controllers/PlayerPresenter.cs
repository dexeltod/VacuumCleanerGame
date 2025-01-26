using System;
using Sources.ControllersInterfaces;
using Sources.PresentationInterfaces;
using Sources.Utils;
using UnityEngine;

namespace Sources.Controllers
{
	[RequireComponent(typeof(Rigidbody))]
	public abstract class PlayerPresenter : MonoBehaviour, IMonoPresenter
	{
		private AnimationHasher _animationHasher;
		private Animator _animator;
		private Rigidbody _body;
		private bool _isMove = true;
		private ParticleSystem _particleSystem;
		private ITransformable _transformable;

		private IUpdatable _updatable;

		private void Awake()
		{
			gameObject.SetActive(false);
		}

		private void Update()
		{
			_updatable?.Update(Time.deltaTime);
			_animator.Play(_isMove ? _animationHasher.Run : _animationHasher.Idle);
		}

		public GameObject GameObject => gameObject;

		public void Initialize(
			ITransformable model,
			Animator animator,
			AnimationHasher animationHasher,
			Rigidbody body
		)
		{
			_body = body ?? throw new ArgumentNullException(nameof(body));
			_animationHasher = animationHasher ?? throw new ArgumentNullException(nameof(animationHasher));
			_transformable = model ?? throw new ArgumentNullException(nameof(model));
			_animator = animator ? animator : throw new ArgumentNullException(nameof(animator));

			if (_transformable is IUpdatable updatable)
				_updatable = updatable;

			enabled = true;

			OnMoved(model.Transform.position);
		}

		public void Disable()
		{
			_transformable.Looked -= OnLookAt;
			_transformable.Moved -= OnMoved;
			_transformable.Destroying -= OnDestroying;
		}

		public void Enable()
		{
			_transformable.Looked += OnLookAt;
			_transformable.Moved += OnMoved;
			_transformable.Destroying += OnDestroying;
		}

		protected void DestroyCompose() => _transformable.Destroy();

		private void OnLookAt(Vector3 direction)
		{
			if (direction != Vector3.zero) _body.rotation = Quaternion.LookRotation(direction);

			_isMove = direction != Vector3.zero;
		}

		private void OnMoved(Vector3 newPosition) => transform.position = newPosition;

		private void OnDestroying() => Destroy(gameObject);
	}
}