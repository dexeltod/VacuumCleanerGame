using System;
using Sources.DomainInterfaces.Entities;
using Sources.DomainInterfaces.Implementations;
using UnityEngine;

namespace Sources.Domain.Settings
{
	[Serializable]
	public class SoundSettings : ISoundSettings
	{
		private const float MinValue = -80f;

		[SerializeField] private EventFloatEntity _entity;

		private float _memorizedValue;

		public SoundSettings(EventFloatEntity entity) => _entity = entity ?? throw new ArgumentNullException(nameof(entity));
		public float Value => _entity.Value;

		public EventFloatEntity Entity => _entity;

		public void SetMasterVolume(float value)
		{
			_entity.Value = value;
		}

		public void Mute()
		{
			_memorizedValue = _entity.Value;
			_entity.Value = MinValue;
		}

		public void Unmute()
		{
			_entity.Value = _memorizedValue;
		}
	}
}
