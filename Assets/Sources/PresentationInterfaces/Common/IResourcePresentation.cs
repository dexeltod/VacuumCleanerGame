namespace Sources.PresentationInterfaces.Common
{
	public interface IResourcePresentation : ICollideable
	{
		int ID { get; }
		void Collect();
	}
}