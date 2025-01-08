using System;
using System.Collections.Generic;
using Plugins.CW.Shared.Common.Required.Scripts;
using UnityEditor;
using UnityEngine;

namespace Plugins.CW.LeanLocalization.Required.Scripts
{
	/// <summary>This contains information about a language, and any of its optional cultures.</summary>
	[Serializable]
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[HelpURL(
		LeanLocalization.HelpUrlPrefix + "LeanLanguage"
	)]
	[AddComponentMenu(
		LeanLocalization.ComponentPathPrefix + "Language"
	)]
	public class LeanLanguage : LeanSource
	{
		/// <summary>The language code used for auto translation.</summary>
		public string TranslationCode
		{
			set { translationCode = value; }
			get { return translationCode; }
		}

		[SerializeField] private string translationCode;

		/// <summary>This culture names for this language (e.g. en-GB, en-US).</summary>
		public List<string> Cultures
		{
			get
			{
				if (cultures == null) cultures = new List<string>();
				return cultures;
			}
		}

		[SerializeField] private List<string> cultures;

		public override void Register()
		{
			if (LeanLocalization.CurrentLanguages.ContainsKey(
				    name
			    ) ==
			    false)
			{
				LeanLocalization.CurrentLanguages.Add(
					name,
					this
				);
			}

			TryAddAlias(
				name,
				name
			);

			if (cultures != null)
			{
				foreach (var culture in cultures)
				{
					TryAddAlias(
						culture,
						name
					);
				}
			}
		}

		private void TryAddAlias(string key, string value)
		{
			if (LeanLocalization.CurrentAliases.ContainsKey(
				    key
			    ) ==
			    false)
			{
				LeanLocalization.CurrentAliases.Add(
					key,
					value
				);
			}
		}
	}

#if UNITY_EDITOR
	[CanEditMultipleObjects]
	[CustomEditor(
		typeof(LeanLanguage)
	)]
	public class LeanLanguage_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			LeanLanguage tgt;
			LeanLanguage[] tgts;
			GetTargets(
				out tgt,
				out tgts
			);

			Draw(
				"translationCode",
				"The language code used for auto translation."
			);

			Separator();

			Draw(
				"cultures",
				"This culture names for this language (e.g. en-GB, en-US)."
			);
		}

		[MenuItem(
			"Assets/Create/Lean/Localization/Lean Language"
		)]
		private static void CreateLanguage()
		{
			CwHelper.CreatePrefabAsset(
				"New Language"
			).AddComponent<LeanLanguage>();
		}
	}

#endif
}
