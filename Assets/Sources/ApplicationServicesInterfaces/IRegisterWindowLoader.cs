using Sources.ServicesInterfaces.Authorization;

namespace Sources.ApplicationServicesInterfaces
{
	public interface IRegisterWindowLoader
	{
		IAuthorization Load();
	}
}