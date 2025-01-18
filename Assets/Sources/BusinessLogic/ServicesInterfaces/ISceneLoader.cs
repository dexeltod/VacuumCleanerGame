using Cysharp.Threading.Tasks;

namespace Sources.BusinessLogic.ServicesInterfaces
{
	public interface ISceneLoader
	{
		UniTask Load(string nextScene);
	}
}
