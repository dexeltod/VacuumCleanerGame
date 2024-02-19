using System;
using Sources.Controllers.Common;
using Sources.Controllers.Mesh;
using Sources.PresentationInterfaces;
using Sources.Utils;
using UnityEngine;

namespace Sources.Infrastructure.Factories.Player
{
	[RequireComponent(typeof(Rigidbody))] public abstract class Presenter : MonoBehaviour
	{
		private Rigidbody _rigidbody;
		private Transformable _model;
		private Animator _animator;

		private IUpdatable _updatable = null;
		private bool _isMove = true;
		private AnimationHasher _animationHasher;
		private ParticleSystem _particleSystem;
		private MeshDeformationController _meshDeformationController;

		public void Initialize(
			Transformable model,
			Rigidbody physic,
			Animator animator,
			AnimationHasher animationHasher
		)
		{
			_animationHasher = animationHasher ?? throw new ArgumentNullException(nameof(animationHasher));
			_model = model ?? throw new ArgumentNullException(nameof(model));
			_rigidbody = physic ? physic : throw new ArgumentNullException(nameof(physic));
			_animator = animator ? animator : throw new ArgumentNullException(nameof(animator));

			if (_model is IUpdatable updatable)
				_updatable = updatable;

			enabled = true;

			OnMoved(model.Transform.position);
		}

		private void OnEnable()
		{
			_model.Looked += OnLookAt;
			_model.Moved += OnMoved;
			_model.Destroying += OnDestroying;
		}

		protected void DestroyCompose() =>
			_model.Destroy();

		private void OnDisable()
		{
			_model.Looked -= OnLookAt;
			_model.Moved -= OnMoved;
			_model.Destroying -= OnDestroying;
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