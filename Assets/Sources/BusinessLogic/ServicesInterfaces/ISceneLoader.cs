using Cysharp.Threading.Tasks;

namespace Sources.BusinessLogic.ServicesInterfaces
{
	public interface ISceneLoader
	{
		void Load(string nextScene);
		UniTask LoadAsync(string nextScene);
	}
}
