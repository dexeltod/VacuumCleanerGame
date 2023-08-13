using System.Collections;
using UnityEngine;

namespace Sources.View.SceneEntity
{
	public class LoadingCurtain : MonoBehaviour
	{
		[SerializeField] private CanvasGroup _curtain;
		
		private void Awake() => 
			DontDestroyOnLoad(this);

		public void Show()
		{
			gameObject.SetActive(true);
			_curtain.alpha = 1;
		}

		public void Hide() => 
			StartCoroutine(HideCurtain());

		private IEnumerator HideCurtain()
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