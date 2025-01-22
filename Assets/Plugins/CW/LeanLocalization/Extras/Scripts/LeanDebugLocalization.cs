using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;

namespace Plugins.CW.LeanLocalization.Extras.Scripts
{
	[DefaultExecutionOrder(
		-100
	)]
	[HelpURL(
		Required.Scripts.LeanLocalization.HelpUrlPrefix + "LeanLocalization"
	)]
	[AddComponentMenu(
		""
	)]
	public class LeanDebugLocalization : MonoBehaviour
	{
		[SerializeField] private StringEvent onString;

		public StringEvent OnString
		{
			get
			{
				if (onString == null) onString = new StringEvent();
				return onString;
			}
		}

		protected virtual void OnEnable()
		{
			Required.Scripts.LeanLocalization.OnLocalizationChanged += HandleLocalizationChanged;
		}

		protected virtual void OnDisable()
		{
			Required.Scripts.LeanLocalization.OnLocalizationChanged -= HandleLocalizationChanged;
		}

		public void ClearSave()
		{
			PlayerPrefs.DeleteKey(
				"LeanLocalization.CurrentLanguage"
			);
		}

		public void ClearSaveAlt()
		{
			PlayerPrefs.DeleteKey(
				"LeanLocalization.CurrentLanguageAlt"
			);
		}

		private void HandleLocalizationChanged()
		{
			var text = "";

			if (Required.Scripts.LeanLocalization.Instances.Count > 0)
			{
				Required.Scripts.LeanLocalization first = Required.Scripts.LeanLocalization.Instances[0];

				text += "LOOKING FOR: ";

				if (first.DetectLanguage == Required.Scripts.LeanLocalization.DetectType.SystemLanguage)
				{
					text += Application.systemLanguage.ToString();
				}
				else if (first.DetectLanguage == Required.Scripts.LeanLocalization.DetectType.CurrentCulture)
				{
					CultureInfo cultureInfo = CultureInfo.CurrentCulture;

					if (cultureInfo != null) text += cultureInfo.Name;
				}
				else if (first.DetectLanguage == Required.Scripts.LeanLocalization.DetectType.CurrentCulture)
				{
					CultureInfo cultureInfo = CultureInfo.CurrentUICulture;

					if (cultureInfo != null) text += cultureInfo.Name;
				}

				text += "\n\n";

				var load = "";

				if (first.SaveLoad == Required.Scripts.LeanLocalization.SaveLoadType.WhenChanged)
					load = PlayerPrefs.GetString(
						"LeanLocalization.CurrentLanguage"
					);
				else if (first.SaveLoad == Required.Scripts.LeanLocalization.SaveLoadType.WhenChanged)
					load = PlayerPrefs.GetString(
						"LeanLocalization.CurrentLanguageAlt"
					);

				if (string.IsNullOrEmpty(
					    load
				    ) ==
				    false)
					text += "LOADING PREVIOUSLY SAVED: " + load;

				text += "\n\nALIASES:\n";

				foreach (KeyValuePair<string, string> alias in Required.Scripts.LeanLocalization.CurrentAliases)
					text += alias.Key + " = " + alias.Value + "\n";

				text += "\n\nDETECTED: " + first.CurrentLanguage;
			}

			if (onString != null)
				onString.Invoke(
					text
				);
		}

		[Serializable]
		public class StringEvent : UnityEvent<string>
		{
		}
	}
}