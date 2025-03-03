using Sources.DomainInterfaces.Implementations;

namespace Sources.DomainInterfaces.Entities
{
	public interface ISoundSettings
	{
		EventFloatEntity Entity { get; }
		float Value { get; }
		void Mute();
		void SetMasterVolume(float value);
		void Unmute();
	}
}