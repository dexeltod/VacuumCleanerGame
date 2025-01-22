using System;
using UnityEngine;

namespace Sources.Utils.ParticleColorChanger.Scripts
{
	[DisallowMultipleComponent]
	public class PS_ColorChanger : MonoBehaviour
	{
		#region Fields

		[Tooltip("Current \"Main\" color of all particle systems")]
		public Color currentColor;

		[Tooltip("New \"Main\" color of all particle systems")]
		public Color newColor;

		private Color currentHSV; // r -> H; g -> S; b -> V (not a really correct way to do it :D)
		private Color newHSV; // r -> H; g -> S; b -> V (not a really correct way to do it :D)

		#endregion

		#region Methods

		/**
			Update colors of all child systems
		*/
		public void ChangeColor()
		{
			ParticleSystem[] systems = GetComponentsInChildren<ParticleSystem>();

			Color.RGBToHSV(currentColor, out currentHSV.r, out currentHSV.g, out currentHSV.b);
			Color.RGBToHSV(newColor, out newHSV.r, out newHSV.g, out newHSV.b);

			foreach (ParticleSystem system in systems)
			{
				ParticleSystem.MainModule main = system.main;

				switch (main.startColor.mode)
				{
					case ParticleSystemGradientMode.Color:
						main.startColor = new ParticleSystem.MinMaxGradient(
							ConvertCurrentToNew(main.startColor.color)
						);
						break;
					case ParticleSystemGradientMode.TwoColors:
						main.startColor = new ParticleSystem.MinMaxGradient(
							ConvertCurrentToNew(main.startColor.colorMin),
							ConvertCurrentToNew(main.startColor.colorMax)
						);
						break;
					case ParticleSystemGradientMode.Gradient:
						main.startColor = new ParticleSystem.MinMaxGradient(
							ConvertCurrentToNew(main.startColor.gradient)
						);
						break;
					case ParticleSystemGradientMode.TwoGradients:
						main.startColor = new ParticleSystem.MinMaxGradient(
							ConvertCurrentToNew(main.startColor.gradientMin),
							ConvertCurrentToNew(main.startColor.gradientMax)
						);
						break;
				}
			}
		}

		/**
			Swap currentColor & newColor
		*/
		public void SwapCurrentWithNewColors()
		{
			Color temp = currentColor;
			currentColor = newColor;
			newColor = temp;
		}

		/**
			Convert a gradient from current color system to the new one
		*/
		public Gradient ConvertCurrentToNew(Gradient gradient)
		{
			var g = new Gradient();
			g.mode = gradient.mode;

			var alphaKeys = new GradientAlphaKey[gradient.alphaKeys.Length];
			var colorKeys = new GradientColorKey[gradient.colorKeys.Length];

			for (var i = 0; i < g.colorKeys.Length; ++i)
				colorKeys[i] = new GradientColorKey(
					ConvertCurrentToNew(gradient.colorKeys[i].color),
					gradient.colorKeys[i].time
				);

			Array.Copy(gradient.alphaKeys, alphaKeys, alphaKeys.Length);

			g.SetKeys(colorKeys, alphaKeys);
			return g;
		}

		/**
			Convert color from current color system to the new one
		*/
		public Color ConvertCurrentToNew(Color color)
		{
			Color hsv;
			Color.RGBToHSV(color, out hsv.r, out hsv.g, out hsv.b);
			Color endRes = Color.HSVToRGB(
				Mathf.Clamp01(Mathf.Abs(newHSV.r + (currentHSV.r - hsv.r))),
				hsv.g,
				hsv.b
			);
			endRes.a = color.a;
			return endRes;
		}

		#endregion
	}
}