using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Sources.Application;

public class YandexGamesSdkController : IYandexGamesSdkController
{
	private readonly ICoroutineRunner _coroutineRunner;

	private bool _isYandexSdkInitialized;

	public YandexGamesSdkController(ICoroutineRunner coroutineRunner)
	{
		_coroutineRunner = coroutineRunner;
	}

	public UniTask Initialize()
	{
		_coroutineRunner.StartCoroutine(
			YandexGamesSdk.Initialize(OnYandexInitializationTrue)
		);

		return UniTask.WaitWhile(() => _isYandexSdkInitialized == false);
	}

	private void OnYandexInitializationTrue() =>
		_isYandexSdkInitialized = true;
}