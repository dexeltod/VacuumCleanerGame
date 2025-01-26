namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface IResource<T>
	{
		T Value { get; set; }
		T MaxValue { get; set; }
		T ReadOnlyMaxValue { get; }
	}
}