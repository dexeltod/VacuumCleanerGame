using Sources.ApplicationServicesInterfaces.Authorization;

namespace Sources.ApplicationServicesInterfaces
{
	public interface IRegisterWindowLoader
	{
		IAuthorization Load();
	}
}