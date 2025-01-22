using System;
using Plugins.CW.Shared.Common.Required.Scripts;
using UnityEditor;
using UnityEngine;

namespace Plugins.CW.LeanCommon.Extras.Scripts
{
	/// <summary>
	///     This component allows you to change the color of the SpriteRenderer attached to the current GameObject when
	///     selected.
	/// </summary>
	[ExecuteInEditMode]
	[RequireComponent(
		typeof(SpriteRenderer)
	)]
	[HelpURL(
		Required.Scripts.LeanCommon.HelpUrlPrefix + "LeanSelectableSpriteRendererColor"
	)]
	[AddComponentMenu(
		Required.Scripts.LeanCommon.ComponentPathPrefix + "Selectable SpriteRenderer Color"
	)]
	public class LeanSelectableSpriteRendererColor : LeanSelectableBehaviour
	{
		[SerializeField] private Color defaultColor = Color.white;

		[SerializeField] private Color selectedColor = Color.green;

		[NonSerialized] private SpriteRenderer cachedSpriteRenderer;

		/// <summary>The default color given to the SpriteRenderer.</summary>
		public Color DefaultColor
		{
			set
			{
				defaultColor = value;
				UpdateColor();
			}
			get => defaultColor;
		}

		/// <summary>The color given to the SpriteRenderer when selected.</summary>
		public Color SelectedColor
		{
			set
			{
				selectedColor = value;
				UpdateColor();
			}
			get => selectedColor;
		}

		protected override void OnSelected(LeanSelect select)
		{
			UpdateColor();
		}

		protected override void OnDeselected(LeanSelect select)
		{
			UpdateColor();
		}

		public void UpdateColor()
		{
			if (cachedSpriteRenderer == null) cachedSpriteRenderer = GetComponent<SpriteRenderer>();

			Color color = Selectable != null && Selectable.IsSelected ? selectedColor : defaultColor;

			cachedSpriteRenderer.color = color;
		}
	}

#if UNITY_EDITOR
	[CanEditMultipleObjects]
	[CustomEditor(
		typeof(LeanSelectableSpriteRendererColor)
	)]
	public class LeanSelectableSpriteRendererColor_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			LeanSelectableSpriteRendererColor tgt;
			LeanSelectableSpriteRendererColor[] tgts;
			GetTargets(
				out tgt,
				out tgts
			);

			var updateColor = false;

			Draw(
				"defaultColor",
				ref updateColor,
				"The default color given to the SpriteRenderer."
			);
			Draw(
				"selectedColor",
				ref updateColor,
				"The color given to the SpriteRenderer when selected."
			);

			if (updateColor)
			{
				serializedObject.ApplyModifiedProperties();

				Each(
					tgts,
					t => t.UpdateColor(),
					true
				);
			}
		}
	}

#endif
}