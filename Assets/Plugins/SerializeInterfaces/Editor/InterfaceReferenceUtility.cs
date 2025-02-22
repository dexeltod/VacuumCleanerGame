﻿using System;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Plugins.SerializeInterfaces.Editor
{
	internal static class InterfaceReferenceUtility
	{
		private const float _helpBoxHeight = 24;

		private static GUIStyle _normalInterfaceLabelStyle;
		private static bool _isOpeningQueued;

		public static float GetPropertyHeight(SerializedProperty property,
			GUIContent label,
			InterfaceObjectArguments args)
		{
			if (IsAssignedAndHasWrongInterface(
				    property.objectReferenceValue,
				    args
			    ))
				return EditorGUIUtility.singleLineHeight + _helpBoxHeight;

			return EditorGUIUtility.singleLineHeight;
		}

		public static bool IsAsset(Type type) =>
			!(type == typeof(GameObject) || type == typeof(Component));

		public static void OnGUI(Rect position,
			SerializedProperty property,
			GUIContent label,
			InterfaceObjectArguments args)
		{
			InitializeStyleIfNeeded();

			Object prevValue = property.objectReferenceValue;
			position.height = EditorGUIUtility.singleLineHeight;
			Color prevColor = GUI.backgroundColor;

			// change visuals if the assigned value doesn't implement the interface (e.g. after removing the interface from the target)
			if (IsAssignedAndHasWrongInterface(
				    prevValue,
				    args
			    ))
			{
				ShowWrongInterfaceErrorBox(
					position,
					prevValue,
					args
				);
				GUI.backgroundColor = Color.red;
			}

			// disable if not assignable from drag and drop
			bool prevEnabledState = GUI.enabled;
			if (Event.current.type == EventType.DragUpdated &&
			    position.Contains(
				    Event.current.mousePosition
			    ) &&
			    GUI.enabled &&
			    !CanAssign(
				    DragAndDrop.objectReferences,
				    args,
				    true
			    ))
				GUI.enabled = false;

			EditorGUI.BeginChangeCheck();
			EditorGUI.ObjectField(
				position,
				property,
				args.ObjectType,
				label
			);

			if (EditorGUI.EndChangeCheck())
			{
				// assign the value from the GameObject if it's dragged in, or reset if the value isn't assignable
				Object newVal = GetClosestAssignableComponent(
					property.objectReferenceValue,
					args
				);
				if (newVal != null &&
				    !CanAssign(
					    newVal,
					    args
				    ))
					property.objectReferenceValue = prevValue;
				property.objectReferenceValue = newVal;
			}

			GUI.backgroundColor = prevColor;
			GUI.enabled = prevEnabledState;

			int controlID = GUIUtility.GetControlID(
				                FocusType.Passive
			                ) -
			                1;
			bool isHovering = position.Contains(
				Event.current.mousePosition
			);
			DrawInterfaceNameLabel(
				position,
				prevValue == null || isHovering ? $"({args.InterfaceType.Name})" : "*",
				controlID
			);
			ReplaceObjectPickerForControl(
				property,
				args,
				controlID
			);
		}

		private static bool
			CanAssign(Object[] objects, InterfaceObjectArguments args, bool lookIntoGameObject = false) =>
			objects.All(
				obj => CanAssign(
					obj,
					args,
					lookIntoGameObject
				)
			);

		private static bool CanAssign(Object obj, InterfaceObjectArguments args, bool lookIntoGameObject = false)
		{
			// We should never pass null, but this catches cases where scripts are broken (deleted/not compiled but still on the GameObject)
			if (obj == null)
				return false;

			if (args.InterfaceType.IsAssignableFrom(
				    obj.GetType()
			    ) &&
			    args.ObjectType.IsAssignableFrom(
				    obj.GetType()
			    ))
				return true;
			if (lookIntoGameObject)
				return CanAssign(
					GetClosestAssignableComponent(
						obj,
						args
					),
					args
				);

			return false;
		}

		private static void DrawInterfaceNameLabel(Rect position, string displayString, int controlID)
		{
			if (Event.current.type == EventType.Repaint)
			{
				const int additionalLeftWidth = 3;
				const int verticalIndent = 1;
				GUIContent content = EditorGUIUtility.TrTextContent(
					displayString
				);
				Vector2 size = _normalInterfaceLabelStyle.CalcSize(
					content
				);
				Rect interfaceLabelPosition = position;
				interfaceLabelPosition.width = size.x + additionalLeftWidth;
				interfaceLabelPosition.x += position.width - interfaceLabelPosition.width - 18;
				interfaceLabelPosition.height = interfaceLabelPosition.height - verticalIndent * 2;
				interfaceLabelPosition.y += verticalIndent;
				_normalInterfaceLabelStyle.Draw(
					interfaceLabelPosition,
					EditorGUIUtility.TrTextContent(
						displayString
					),
					controlID,
					DragAndDrop.activeControlID == controlID,
					false
				);
			}
		}

		/// <summary>
		///     Gets itself if assignable, otherwise will get the root gameobject if it belongs to one, and return the first
		///     possible component
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private static Object GetClosestAssignableComponent(Object obj, InterfaceObjectArguments args)
		{
			if (CanAssign(
				    obj,
				    args
			    ))
				return obj;

			if (args.ObjectType.IsSubclassOf(
				    typeof(Component)
			    ))
			{
				if (obj is GameObject go &&
				    TryFindSuitableComponent(
					    go,
					    args,
					    out Component foundComponent
				    ))
					return foundComponent;
				if (obj is Component comp &&
				    TryFindSuitableComponent(
					    comp.gameObject,
					    args,
					    out foundComponent
				    ))
					return foundComponent;
			}

			return null;
		}

		private static void InitializeStyleIfNeeded()
		{
			if (_normalInterfaceLabelStyle != null)
				return;

			_normalInterfaceLabelStyle = new GUIStyle(
				EditorStyles.label
			);
			GUIStyle objectFieldStyle = EditorStyles.objectField;
			_normalInterfaceLabelStyle.font = objectFieldStyle.font;
			_normalInterfaceLabelStyle.fontSize = objectFieldStyle.fontSize;
			_normalInterfaceLabelStyle.fontStyle = objectFieldStyle.fontStyle;
			_normalInterfaceLabelStyle.alignment = TextAnchor.MiddleRight;
			_normalInterfaceLabelStyle.padding = new RectOffset(
				0,
				2,
				0,
				0
			);
			var texture = new Texture2D(
				1,
				1
			);
			texture.SetPixel(
				0,
				0,
				new Color(
					40 / 255f,
					40 / 255f,
					40 / 255f
				)
			);
			texture.Apply();
			_normalInterfaceLabelStyle.normal.background = texture;
		}

		private static bool IsAssignedAndHasWrongInterface(Object obj, InterfaceObjectArguments args) =>
			obj != null &&
			!args.InterfaceType.IsAssignableFrom(
				obj.GetType()
			);

		private static void OpenDelayed(SerializedProperty property, InterfaceObjectArguments args)
		{
			EditorWindow win = EditorWindow.focusedWindow;
			win.Close();

			TypeCache.TypeCollection derivedTypes = TypeCache.GetTypesDerivedFrom(
				args.InterfaceType
			);
			var sb = new StringBuilder();

			foreach (Type type in derivedTypes)
				if (args.ObjectType.IsAssignableFrom(
					    type
				    ))
					sb.Append(
						"t:" + type.FullName + " "
					);

			// this makes sure we don't find anything if there's no type supplied
			if (sb.Length == 0)
				sb.Append(
					"t:"
				);

			var filter = new ObjectSelectorFilter(
				sb.ToString(),
				obj => CanAssign(
					obj,
					args
				)
			);
			ObjectSelectorWindow.Show(
				property,
				obj =>
				{
					property.objectReferenceValue = obj;
					property.serializedObject.ApplyModifiedProperties();
				},
				(obj, success) =>
				{
					if (success) property.objectReferenceValue = obj;
				},
				filter
			);
			ObjectSelectorWindow.Instance.position = win.position;
			var content = new GUIContent(
				$"Select {args.ObjectType.Name} ({args.InterfaceType.Name})"
			);
			ObjectSelectorWindow.Instance.titleContent = content;
			_isOpeningQueued = false;
		}

		private static void ReplaceObjectPickerForControl(SerializedProperty property,
			InterfaceObjectArguments args,
			int controlID)
		{
			int currentObjectPickerID = EditorGUIUtility.GetObjectPickerControlID();

			if (controlID == currentObjectPickerID && _isOpeningQueued == false)
				if (EditorWindow.focusedWindow != null)
				{
					_isOpeningQueued = true;
					EditorApplication.delayCall += () => OpenDelayed(
						property,
						args
					);
				}
		}

		private static void ShowWrongInterfaceErrorBox(Rect position, Object prevValue, InterfaceObjectArguments args)
		{
			Rect helpBoxPosition = position;
			helpBoxPosition.y += position.height;
			helpBoxPosition.height = _helpBoxHeight;
			EditorGUI.HelpBox(
				helpBoxPosition,
				$"Object {prevValue.name} needs to implement the required interface {args.InterfaceType}.",
				MessageType.Error
			);
		}

		private static bool TryFindSuitableComponent(GameObject go,
			InterfaceObjectArguments args,
			out Component component)
		{
			foreach (Component comp in go.GetComponents(
				         args.ObjectType
			         ))
				if (CanAssign(
					    comp,
					    args
				    ))
				{
					component = comp;
					return true;
				}

			component = null;
			return false;
		}
	}

	public struct InterfaceObjectArguments
	{
		public Type ObjectType;
		public Type InterfaceType;

		public InterfaceObjectArguments(Type objectType, Type interfaceType)
		{
			Debug.Assert(
				typeof(Object).IsAssignableFrom(
					objectType
				),
				$"{nameof(objectType)} needs to be of Type {typeof(Object)}."
			);
			Debug.Assert(
				interfaceType.IsInterface,
				$"{nameof(interfaceType)} needs to be an interface."
			);
			ObjectType = objectType;
			InterfaceType = interfaceType;
		}
	}
}