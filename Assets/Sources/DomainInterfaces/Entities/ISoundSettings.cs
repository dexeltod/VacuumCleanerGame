namespace Sources.DomainInterfaces.Entities
{
	public interface ISoundSettings
	{
		float MasterVolume { get; }
		void SetMasterVolume(float value);
	}
}