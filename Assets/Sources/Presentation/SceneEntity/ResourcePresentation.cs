using System;
using Sources.ControllersInterfaces;
using Sources.PresentationInterfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sources.Presentation.SceneEntity
{
	public class ResourcePresentation : MonoBehaviour, ICollectable
	{
		[SerializeField] private GameObject _view;

		[SerializeField] private ParticleSystem _particle;

		[Header("Sound")] [SerializeField] private AudioSource _sound;

		[Range(0.6f, 0.8f)] [SerializeField] private float _minPitchRange = 0.6f;
		[Range(0.6f, 0.8f)] [SerializeField] private float _maxPitchRange = 0.8f;
		[Range(0.6f, 1)] [SerializeField] private float _soundVolume = 0.6f;

		private int _score = 1;
		private IResourcesProgressPresenter _resourcesProgressPresenterProvider;

		public int Score => _score;
		public GameObject View => _view;

		public ParticleSystem Particle => _particle;

		public AudioSource Sound => _sound;

		public event Action Collected;

		public void HandleCollide(Collision collision)
		{
			if (_resourcesProgressPresenterProvider.IsMaxScoreReached == false)
				return;

			if (collision.collider.name is not ("VacuumColliderBottom" or "VacuumColliderTop")) return;

			_resourcesProgressPresenterProvider.TryAddSand(_score);
			_view.SetActive(false);
			_particle.Play();
			_sound.Play();
			Collected?.Invoke();
		}

		public void Construct(IResourcesProgressPresenter resourcesProgressPresenterProvider, int score)
		{
			if (score <= 0) throw new ArgumentOutOfRangeException(nameof(score));

			_score = score;
			_resourcesProgressPresenterProvider = resourcesProgressPresenterProvider;
			_sound.pitch = Random.Range(_minPitchRange, _maxPitchRange);
			_sound.volume = _soundVolume;
		}
	}
}
