﻿using System;
using Plugins.SerializeInterfaces.Runtime;
using UnityEditor;
using UnityEngine;

namespace Plugins.SerializeInterfaces.Editor
{
	[CustomPropertyDrawer(
		typeof(RequireInterfaceAttribute)
	)]
	public class RequireInterfaceAttributeDrawer : PropertyDrawer
	{
		private RequireInterfaceAttribute _requireInterfaceAttribute => (RequireInterfaceAttribute)attribute;

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var args = new InterfaceObjectArguments(
				GetTypeOrElementType(
					fieldInfo.FieldType
				),
				_requireInterfaceAttribute.InterfaceType
			);
			return InterfaceReferenceUtility.GetPropertyHeight(
				property,
				label,
				args
			);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var args = new InterfaceObjectArguments(
				GetTypeOrElementType(
					fieldInfo.FieldType
				),
				_requireInterfaceAttribute.InterfaceType
			);
			InterfaceReferenceUtility.OnGUI(
				position,
				property,
				label,
				args
			);
		}

		/// <summary>
		///     returns the type, or if it's a container, returns the type of the element.
		/// </summary>
		private Type GetTypeOrElementType(Type type)
		{
			if (type.IsArray)
				return type.GetElementType();
			if
				(type.IsGenericType) // this assumes it's a list or any other container type with a generic parameter (it's for future proofing)
				return type.GetGenericArguments()[0];

			return type;
		}
	}
}