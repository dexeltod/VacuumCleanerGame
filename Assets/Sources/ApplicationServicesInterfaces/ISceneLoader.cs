using Cysharp.Threading.Tasks;

namespace Sources.Application
{
	public interface ISceneLoader
	{
		UniTask Load(string nextScene);
	}
}