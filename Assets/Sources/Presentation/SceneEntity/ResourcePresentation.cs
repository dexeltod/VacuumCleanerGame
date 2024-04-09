using System;
using Sources.ControllersInterfaces;
using Sources.InfrastructureInterfaces.Providers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sources.Presentation.SceneEntity
{
	public class ResourcePresentation : MonoBehaviour
	{
		[SerializeField] private GameObject _view;

		[SerializeField] private ParticleSystem _particle;

		[Header("Sound")] [SerializeField] private AudioSource _sound;

		[Range(0.6f, 0.8f)] [SerializeField] private float _minPitchRange = 0.6f;
		[Range(0.6f, 0.8f)] [SerializeField] private float _maxPitchRange = 0.8f;
		[Range(0.6f, 1)] [SerializeField] private float _soundVolume = 0.6f;

		private int _score = 1;
		private IResourcesProgressPresenterProvider _resourcesProgressPresenterProvider;

		public int Score => _score;
		public GameObject View => _view;

		public ParticleSystem Particle => _particle;

		public AudioSource Sound => _sound;

		private IResourcesProgressPresenter ResourcesProgressPresenter =>
			_resourcesProgressPresenterProvider.Implementation;

		public void HandleCollide(Collision collision)
		{
			if (ResourcesProgressPresenter.IsMaxScoreReached == false)
				return;

			if (collision.collider.name is not ("VacuumColliderBottom" or "VacuumColliderTop")) return;

			_resourcesProgressPresenterProvider.Implementation.TryAddSand(_score);
			_view.SetActive(false);
			_particle.Play();
			_sound.Play();
		}

		public void Construct(IResourcesProgressPresenterProvider resourcesProgressPresenterProvider, int score)
		{
			if (score <= 0) throw new ArgumentOutOfRangeException(nameof(score));
			_score = score;
			_resourcesProgressPresenterProvider = resourcesProgressPresenterProvider;
			_sound.pitch = Random.Range(_minPitchRange, _maxPitchRange);
			_sound.volume = _soundVolume;
		}
	}
}