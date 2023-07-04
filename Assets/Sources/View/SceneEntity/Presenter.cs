using Domain.Scene;
using UnityEngine;

namespace View.SceneEntity
{
	[RequireComponent(typeof(Rigidbody))]
	public abstract class Presenter : MonoBehaviour
	{
		private Rigidbody _rigidbody;
		private Transformable _model;

		private IUpdatable _updatable = null;
		private bool _isMove = true;

		public Transformable Model => _model;

		public void Init(Transformable model, Rigidbody rigidbody)
		{
			_rigidbody = rigidbody;
			_model = model;

			if (_model is IUpdatable)
				_updatable = (IUpdatable)_model;

			enabled = true;

			OnMoved(model.Transform.position);
		}

		private void OnEnable()
		{
			_model.MovedPhysics += OnMovedPhysics;
			_model.Looked += OnLookAt;
			_model.Moved += OnMoved;
			_model.Destroying += OnDestroying;
		}

		protected void DestroyCompose() =>
			_model.Destroy();

		private void OnMovedPhysics(Vector3 obj)
		{
			throw new System.NotImplementedException();
		}

		private void OnDisable()
		{
			_model.Looked -= OnLookAt;
			_model.Moved -= OnMoved;
			_model.Destroying -= OnDestroying;
		}

		private void Update()
		{
			if (_isMove) 
				_updatable?.Update(Time.deltaTime);
		}

		private void OnLookAt(Vector3 direction)
		{
			transform.rotation = Quaternion.LookRotation(direction);
		}

		private void OnMoved(Vector3 newPosition) =>
			transform.position = newPosition;

		private void OnDestroying() =>
			Destroy(gameObject);
	}
}