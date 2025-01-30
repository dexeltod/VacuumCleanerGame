namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface IResource<T> : IReadOnlyProgress<T>
	{
		T Value { get; set; }
		T MaxValue { get; set; }
	}
}
