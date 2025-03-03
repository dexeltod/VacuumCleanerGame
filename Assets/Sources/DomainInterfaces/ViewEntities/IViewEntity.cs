namespace Sources.DomainInterfaces.ViewEntities
{
	public interface IViewEntity : IViewEvent
	{
		void SetActive(bool isEnabled);
	}
}
