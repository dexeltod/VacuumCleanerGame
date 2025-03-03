using System;
using System.Collections.Generic;
using Plugins.CW.Shared.Common.Required.Scripts;
using UnityEditor;
using UnityEngine;

namespace Plugins.CW.LeanLocalization.Required.Scripts
{
	/// <summary>
	///     The class stores a token name (e.g. "AGE"), allowing it to be replaced with the token value (e.g. "20").
	///     To use the token in your text, simply include the token name surrounded by braces (e.g. "I am {AGE} years old!")
	/// </summary>
	[ExecuteInEditMode]
	[HelpURL(
		LeanLocalization.HelpUrlPrefix + "LeanToken"
	)]
	[AddComponentMenu(
		LeanLocalization.ComponentPathPrefix + "Token"
	)]
	public class LeanToken : LeanSource
	{
		[NonSerialized] private readonly static HashSet<ILocalizationHandler> tempHandlers = new();
		[SerializeField] private string value;

		[NonSerialized] private HashSet<ILocalizationHandler> handlers;

		/// <summary>
		///     This is the current value/text for this token. When this changes, it will automatically update all
		///     localizations that use this token.
		/// </summary>
		public string Value
		{
			set
			{
				if (this.value != value)
				{
					this.value = value;

					if (handlers != null)
					{
						tempHandlers.Clear();

						tempHandlers.UnionWith(
							handlers
						);

						foreach (ILocalizationHandler handler in tempHandlers) handler.UpdateLocalization();
					}
				}
			}

			get => value;
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			UnregisterAll();
		}

		/// <summary>This method allows you to set <b>Value</b> from an inspector event using a <b>float</b> value.</summary>
		public void SetValue(float value)
		{
			Value = value.ToString();
		}

		/// <summary>This method allows you to set <b>Value</b> from an inspector event using a <b>string</b> value.</summary>
		public void SetValue(string value)
		{
			Value = value;
		}

		/// <summary>This method allows you to set <b>Value</b> from an inspector event using an <b>int</b> value.</summary>
		public void SetValue(int value)
		{
			Value = value.ToString();
		}

		public void Register(ILocalizationHandler handler)
		{
			if (handler != null)
			{
				if (handlers == null) handlers = new HashSet<ILocalizationHandler>();

				handlers.Add(
					handler
				);
			}
		}

		public void Unregister(ILocalizationHandler handler)
		{
			if (handlers != null)
				handlers.Remove(
					handler
				);
		}

		public void UnregisterAll()
		{
			if (handlers != null)
			{
				foreach (ILocalizationHandler handler in handlers)
					handler.Unregister(
						this
					);

				handlers.Clear();
			}
		}

		public override void Register()
		{
			LeanLocalization.RegisterToken(
				name,
				this
			);
		}
	}

#if UNITY_EDITOR
	[CanEditMultipleObjects]
	[CustomEditor(
		typeof(LeanToken)
	)]
	public class LeanToken_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			LeanToken tgt;
			LeanToken[] tgts;
			GetTargets(
				out tgt,
				out tgts
			);

			if (Draw(
				    "value",
				    "This is the current value/text for this token. When this changes, it will automatically update all localizations that use this token."
			    ))
				Each(
					tgts,
					t => t.Value = serializedObject.FindProperty(
						"value"
					).stringValue
				);
		}

		[MenuItem(
			"Assets/Create/Lean/Localization/Lean Token"
		)]
		private static void CreateToken()
		{
			CwHelper.CreatePrefabAsset(
				"New Token"
			).AddComponent<LeanToken>();
		}
	}

#endif
}