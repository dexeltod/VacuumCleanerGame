using UnityEngine;
using UnityEngine.Events;

namespace Plugins.CW.LeanLocalization.Extras.Scripts
{
	[DefaultExecutionOrder(-100)]
	[HelpURL(Required.Scripts.LeanLocalization.HelpUrlPrefix + "LeanLocalization")]
	[AddComponentMenu("")]
	public class LeanDebugLocalization : MonoBehaviour
	{
		[System.Serializable] public class StringEvent : UnityEvent<string> {}

		public StringEvent OnString { get { if (onString == null) onString = new StringEvent(); return onString; } } [SerializeField] private StringEvent onString;

		public void ClearSave()
		{
			PlayerPrefs.DeleteKey("LeanLocalization.CurrentLanguage");
		}

		public void ClearSaveAlt()
		{
			PlayerPrefs.DeleteKey("LeanLocalization.CurrentLanguageAlt");
		}

		protected virtual void OnEnable()
		{
			Required.Scripts.LeanLocalization.OnLocalizationChanged += HandleLocalizationChanged;
		}

		protected virtual void OnDisable()
		{
			Required.Scripts.LeanLocalization.OnLocalizationChanged -= HandleLocalizationChanged;
		}

		private void HandleLocalizationChanged()
		{
			var text = "";

			if (Required.Scripts.LeanLocalization.Instances.Count > 0)
			{
				var first = Required.Scripts.LeanLocalization.Instances[0];

				text += "LOOKING FOR: ";

				if (first.DetectLanguage == Required.Scripts.LeanLocalization.DetectType.SystemLanguage)
				{
					text += Application.systemLanguage.ToString();
				}
				else if (first.DetectLanguage == Required.Scripts.LeanLocalization.DetectType.CurrentCulture)
				{
					var cultureInfo = System.Globalization.CultureInfo.CurrentCulture;

					if (cultureInfo != null)
					{
						text += cultureInfo.Name;
					}
				}
				else if (first.DetectLanguage == Required.Scripts.LeanLocalization.DetectType.CurrentCulture)
				{
					var cultureInfo = System.Globalization.CultureInfo.CurrentUICulture;

					if (cultureInfo != null)
					{
						text += cultureInfo.Name;
					}
				}

				text += "\n\n";

				var load = "";

				if (first.SaveLoad == Required.Scripts.LeanLocalization.SaveLoadType.WhenChanged)
				{
					load = PlayerPrefs.GetString("LeanLocalization.CurrentLanguage");
				}
				else if (first.SaveLoad == Required.Scripts.LeanLocalization.SaveLoadType.WhenChanged)
				{
					load = PlayerPrefs.GetString("LeanLocalization.CurrentLanguageAlt");
				}

				if (string.IsNullOrEmpty(load) == false)
				{
					text += "LOADING PREVIOUSLY SAVED: " + load;
				}

				text += "\n\nALIASES:\n";

				foreach (var alias in Required.Scripts.LeanLocalization.CurrentAliases)
				{
					text += alias.Key + " = " + alias.Value + "\n";
				}

				text += "\n\nDETECTED: " + first.CurrentLanguage;
			}

			if (onString != null)
			{
				onString.Invoke(text);
			}
		}
	}
}