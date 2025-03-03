using System;
using Plugins.CW.Shared.Common.Required.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.CW.LeanCommon.Extras.Scripts
{
	/// <summary>
	///     This component allows you to change the color of the Graphic (e.g. Image) attached to the current GameObject
	///     when selected.
	/// </summary>
	[ExecuteInEditMode]
	[RequireComponent(
		typeof(Graphic)
	)]
	[HelpURL(
		Required.Scripts.LeanCommon.HelpUrlPrefix + "LeanSelectableGraphicColor"
	)]
	[AddComponentMenu(
		Required.Scripts.LeanCommon.ComponentPathPrefix + "Selectable Graphic Color"
	)]
	public class LeanSelectableGraphicColor : LeanSelectableBehaviour
	{
		[SerializeField] private Color defaultColor = Color.white;

		[SerializeField] private Color selectedColor = Color.green;

		[NonSerialized] private Graphic cachedGraphic;

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
			if (cachedGraphic == null) cachedGraphic = GetComponent<Graphic>();

			Color color = Selectable != null && Selectable.IsSelected ? selectedColor : defaultColor;

			cachedGraphic.color = color;
		}
	}

#if UNITY_EDITOR
	[CanEditMultipleObjects]
	[CustomEditor(
		typeof(LeanSelectableGraphicColor)
	)]
	public class LeanSelectableGraphicColor_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			LeanSelectableGraphicColor tgt;
			LeanSelectableGraphicColor[] tgts;
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
				Each(
					tgts,
					t => t.UpdateColor(),
					true
				);
		}
	}

#endif
}