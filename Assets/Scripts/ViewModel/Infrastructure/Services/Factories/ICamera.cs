namespace ViewModel.Infrastructure.Services.Factories
{
	public interface ICamera : IService
	{
		UnityEngine.Camera Camera { get; }
	}
}