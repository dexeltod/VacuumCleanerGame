using Cysharp.Threading.Tasks;

namespace Sources.ApplicationServicesInterfaces
{
	public interface ISceneLoader
	{
		UniTask Load(string nextScene);
	}
}