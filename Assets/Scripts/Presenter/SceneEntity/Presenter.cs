using Model.SceneEntity;
using UnityEngine;

namespace Presenter.SceneEntity
{
	public abstract class Presenter : MonoBehaviour
	{
		private Transformable _model;

		private IUpdatable _updatable = null;

		public Transformable Model => _model;

		public void Init(Transformable model)
		{
			_model = model;

			if (_model is IUpdatable)
				_updatable = (IUpdatable)_model;

			enabled = true;

			OnMoved(model.Position);
		}

		private void OnEnable()
		{
			_model.Looked += OnLookAt;
			_model.Moved += OnMoved;
			_model.Destroying += OnDestroying;
		}

		private void OnDisable()
		{
			_model.Looked -= OnLookAt;
			_model.Moved -= OnMoved;
			_model.Destroying -= OnDestroying;
		}

		private void Update() => _updatable?.Update(Time.deltaTime);

		private void OnLookAt(Vector3 direction)
		{
			transform.rotation = Quaternion.LookRotation(direction);
		}

		private void OnMoved(Vector3 newPosition) =>
			transform.position = newPosition;

		private void OnDestroying() =>
			Destroy(gameObject);

		protected void DestroyCompose() =>
			_model.Destroy();
	}
}