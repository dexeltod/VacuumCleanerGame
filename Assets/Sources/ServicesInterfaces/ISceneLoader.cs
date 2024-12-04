using Cysharp.Threading.Tasks;

namespace Sources.ServicesInterfaces
{
	public interface ISceneLoader
	{
		UniTask Load(string nextScene);
	}
}