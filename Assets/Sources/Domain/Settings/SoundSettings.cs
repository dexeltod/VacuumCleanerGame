using System;
using Sources.DomainInterfaces.Entities;
using UnityEngine;

namespace Sources.Domain.Settings
{
	[Serializable]
	public class SoundSettings : ISoundSettings
	{
		[SerializeField] private float _masterVolume;

		public float MasterVolume => _masterVolume;

		public SoundSettings(float masterVolume)
		{
			_masterVolume = masterVolume;
		}

		public void SetMasterVolume(float value)
		{
			_masterVolume = value;
		}
	}
}
