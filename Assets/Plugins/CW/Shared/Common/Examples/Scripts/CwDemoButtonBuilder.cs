﻿using Plugins.CW.Shared.Common.Required.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.CW.Shared.Common.Examples.Scripts
{
	/// <summary>This component allows you to quickly build a UI button to activate only this GameObject when clicked.</summary>
	[HelpURL(
		CwShared.HelpUrlPrefix + "CwDemoButtonBuilder"
	)]
	[AddComponentMenu(
		CwShared.ComponentMenuPrefix + "Demo Button Builder"
	)]
	public class CwDemoButtonBuilder : MonoBehaviour
	{
		[SerializeField] private GameObject buttonPrefab;

		[SerializeField] private RectTransform buttonRoot;

		[SerializeField] private Sprite icon;

		[SerializeField] private Color color = Color.white;

		[SerializeField]
		[Multiline(
			3
		)]
		private string overrideName;

		[SerializeField] private GameObject clone;

		/// <summary>The built button will be based on this prefab.</summary>
		public GameObject ButtonPrefab
		{
			set => buttonPrefab = value;
			get => buttonPrefab;
		}

		/// <summary>The built button will be placed under this transform.</summary>
		public RectTransform ButtonRoot
		{
			set => buttonRoot = value;
			get => buttonRoot;
		}

		/// <summary>The icon given to this button.</summary>
		public Sprite Icon
		{
			set => icon = value;
			get => icon;
		}

		/// <summary>The icon will be tinted by this.</summary>
		public Color Color
		{
			set => color = value;
			get => color;
		}

		/// <summary>Use a different name for the button text?</summary>
		public string OverrideName
		{
			set => overrideName = value;
			get => overrideName;
		}

		[ContextMenu(
			"Build"
		)]
		public void Build()
		{
			if (clone != null)
				DestroyImmediate(
					clone
				);

			if (buttonPrefab != null)
			{
				clone = DoInstantiate();

				clone.name = name;

				var image = clone.GetComponent<Image>();

				if (image != null)
				{
					image.sprite = icon;
					image.color = color;
				}

				var title = clone.GetComponentInChildren<Text>();

				if (title != null)
					title.text = string.IsNullOrEmpty(
						             overrideName
					             ) ==
					             false
						? overrideName
						: name;

				var isolate = clone.GetComponent<CwDemoButton>();

				if (isolate != null) isolate.IsolateTarget = transform;
			}
		}

		[ContextMenu(
			"Build All"
		)]
		public void BuildAll()
		{
			foreach (CwDemoButtonBuilder builder in transform.parent.GetComponentsInChildren<CwDemoButtonBuilder>(
				         true
			         ))
				builder.Build();
		}

		private GameObject DoInstantiate()
		{
#if UNITY_EDITOR
			if (Application.isPlaying == false)
				return (GameObject)PrefabUtility.InstantiatePrefab(
					buttonPrefab,
					buttonRoot
				);
#endif
			return Instantiate(
				buttonPrefab,
				buttonRoot,
				false
			);
		}
	}

#if UNITY_EDITOR
	[CanEditMultipleObjects]
	[CustomEditor(
		typeof(CwDemoButtonBuilder)
	)]
	public class CwDemoButtonBuilder_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			CwDemoButtonBuilder tgt;
			CwDemoButtonBuilder[] tgts;
			GetTargets(
				out tgt,
				out tgts
			);

			Draw(
				"buttonPrefab",
				"The built button will be based on this prefab."
			);
			Draw(
				"buttonRoot",
				"The built button will be placed under this transform."
			);

			Separator();

			Draw(
				"icon",
				"The icon given to this button."
			);
			Draw(
				"color",
				"The icon will be tinted by this."
			);
			Draw(
				"overrideName",
				"Use a different name for the button text?"
			);

			Separator();

			if (Button(
				    "Build All"
			    ))
			{
				Undo.RecordObjects(
					tgts,
					"Build All"
				);

				tgt.BuildAll();
			}
		}
	}

#endif
}