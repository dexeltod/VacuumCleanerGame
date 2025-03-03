using System;
using Sources.PresentationInterfaces.Common;
using Sources.Utils;
using Sources.Utils.ParticleColorChanger.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sources.Presentation.SceneEntity
{
	public class ResourcePresentation : MonoBehaviour, IResourcePresentation
	{
		[SerializeField] private GameObject _view;

		[SerializeField] private ParticleSystem _particle;

		[Header("Sound")] [SerializeField] private AudioSource _sound;

		[Range(0.6f, 0.8f)] [SerializeField] private float _minPitchRange = 0.6f;
		[Range(0.6f, 0.8f)] [SerializeField] private float _maxPitchRange = 0.8f;
		[Range(0.6f, 1)] [SerializeField] private float _soundVolume = 0.6f;

		public GameObject View => _view;

		public int ID { get; private set; }

		public event Action<int> Collided;

		public void Collect()
		{
			_view.SetActive(false);
			_particle.Play();
			_sound.Play();
		}

		public void HandleCollide(Collider collided)
		{
			if (LayerService.GetNameByType(LayerType.Player) == LayerMask.LayerToName(collided.gameObject.layer))
				Collided!.Invoke(ID);
		}

		public void Construct(int id, Material material, Color materialColor)
		{
			if (id < 0) throw new ArgumentOutOfRangeException(nameof(id));

			var colorChanger = _particle.GetComponent<PS_ColorChanger>();

			var particleSystemRenderer = _particle.GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>();

			particleSystemRenderer.material = new Material(material) { color = materialColor };

			colorChanger.newColor = materialColor;
			colorChanger.ChangeColor();

			ID = id;
			_sound.pitch = Random.Range(_minPitchRange, _maxPitchRange);
			_sound.volume = _soundVolume;
		}
	}
}