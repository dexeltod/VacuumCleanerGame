using System.Collections;
using Sources.PresentationInterfaces;
using TMPro;
using UnityEngine;

namespace Sources.Presentation.SceneEntity
{
	public class LoadingCurtain : MonoBehaviour, ILoadingCurtain
	{
		[SerializeField] private CanvasGroup _curtain;
		[SerializeField] private TextMeshProUGUI _infoText;

		private void Awake() => DontDestroyOnLoad(gameObject);

		public void SetText(string empty) => _infoText.text = empty;

		public void Show()
		{
			gameObject.SetActive(true);
			_curtain.alpha = 1;
		}

		public void Hide()
		{
			gameObject.SetActive(false);
			_curtain.alpha = 0;
		}

		public void HideSlowly() => StartCoroutine(StartHiding());

		private IEnumerator StartHiding()
		{
			var waitForSeconds = new WaitForSeconds(0.01F);

			while (_curtain.alpha > 0)
			{
				_curtain.alpha -= 0.03F;
				yield return waitForSeconds;
			}

			gameObject.SetActive(false);
		}
	}
}