using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Plugins.SerializeInterfaces.Runtime;
using UnityEditor;
using UnityEngine;

namespace Plugins.SerializeInterfaces.Editor
{
	[CustomPropertyDrawer(
		typeof(InterfaceReference<>)
	)]
	[CustomPropertyDrawer(
		typeof(InterfaceReference<,>)
	)]
	public class InterfaceReferenceDawer : PropertyDrawer
	{
		private const string _fieldName = "_underlyingValue";

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			SerializedProperty prop = property.FindPropertyRelative(
				_fieldName
			);
			return InterfaceReferenceUtility.GetPropertyHeight(
				prop,
				label,
				GetArguments(
					fieldInfo
				)
			);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty prop = property.FindPropertyRelative(
				_fieldName
			);
			InterfaceReferenceUtility.OnGUI(
				position,
				prop,
				label,
				GetArguments(
					fieldInfo
				)
			);
		}

		private static InterfaceObjectArguments GetArguments(FieldInfo fieldInfo)
		{
			GetObjectAndInterfaceType(
				fieldInfo.FieldType,
				out Type objectType,
				out Type interfaceType
			);
			return new InterfaceObjectArguments(
				objectType,
				interfaceType
			);
		}

		private static void GetObjectAndInterfaceType(Type fieldType, out Type objectType, out Type interfaceType)
		{
			if (TryGetTypesFromInterfaceReference(
				    fieldType,
				    out objectType,
				    out interfaceType
			    ))
				return;

			TryGetTypesFromList(
				fieldType,
				out objectType,
				out interfaceType
			);
		}

		private static bool TryGetTypesFromInterfaceReference(Type fieldType, out Type objectType, out Type interfaceType)
		{
			Type fieldBaseType = fieldType;
			if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(InterfaceReference<>))
				fieldBaseType = fieldType.BaseType;

			if (fieldBaseType.IsGenericType && fieldBaseType.GetGenericTypeDefinition() == typeof(InterfaceReference<,>))
			{
				Type[] types = fieldBaseType.GetGenericArguments();
				interfaceType = types[0];
				objectType = types[1];
				return true;
			}

			objectType = null;
			interfaceType = null;
			return false;
		}

		private static bool TryGetTypesFromList(Type fieldType, out Type objectType, out Type interfaceType)
		{
			Type listType = fieldType.GetInterfaces().FirstOrDefault(
				x =>
					x.IsGenericType &&
					x.GetGenericTypeDefinition() == typeof(IList<>)
			);

			return TryGetTypesFromInterfaceReference(
				listType.GetGenericArguments()[0],
				out objectType,
				out interfaceType
			);
		}
	}
}