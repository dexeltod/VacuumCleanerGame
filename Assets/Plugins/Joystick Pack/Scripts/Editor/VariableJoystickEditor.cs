﻿using Plugins.Joystick_Pack.Scripts.Joysticks;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace Plugins.Joystick_Pack.Scripts.Editor
{
	[CustomEditor(
		typeof(VariableJoystick)
	)]
	public class VariableJoystickEditor : JoystickEditor
	{
		private SerializedProperty joystickType;
		private SerializedProperty moveThreshold;

		protected override void OnEnable()
		{
			base.OnEnable();
			moveThreshold = serializedObject.FindProperty(
				"moveThreshold"
			);
			joystickType = serializedObject.FindProperty(
				"joystickType"
			);
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (background != null)
			{
				var backgroundRect = (RectTransform)background.objectReferenceValue;
				backgroundRect.pivot = center;
			}
		}

		protected override void DrawValues()
		{
			base.DrawValues();
			EditorGUILayout.PropertyField(
				moveThreshold,
				new GUIContent(
					"Move Threshold",
					"The distance away from the center input has to be before the joystick begins to move."
				)
			);
			EditorGUILayout.PropertyField(
				joystickType,
				new GUIContent(
					"Joystick Type",
					"The type of joystick the variable joystick is current using."
				)
			);
		}
	}
}

#endif