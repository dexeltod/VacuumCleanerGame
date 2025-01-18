using Cysharp.Threading.Tasks;

namespace Sources.BuisenessLogic.ServicesInterfaces
{
	public interface ISceneLoader
	{
		UniTask Load(string nextScene);
	}
}