﻿using Plugins.Joystick_Pack.Scripts.Joysticks;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace Plugins.Joystick_Pack.Scripts.Editor
{
	[CustomEditor(
		typeof(FloatingJoystick)
	)]
	public class FloatingJoystickEditor : JoystickEditor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (background != null)
			{
				var backgroundRect = (RectTransform)background.objectReferenceValue;
				backgroundRect.anchorMax = Vector2.zero;
				backgroundRect.anchorMin = Vector2.zero;
				backgroundRect.pivot = center;
			}
		}
	}
}

#endif