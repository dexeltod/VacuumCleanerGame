using System;
using Plugins.CW.Shared.Common.Required.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Plugins.CW.LeanCommon.Extras.Scripts
{
	/// <summary>
	///     This component allows you to convert values like ints and floats into formatted text that can be shown in the UI.
	///     To use this component, simply call one of the <b>SetString</b> methods, and it will output the formatted string to
	///     the <b>OnString</b> event, which can be connected to UI text, etc.</b></summary>
	[HelpURL(
		Required.Scripts.LeanCommon.HelpUrlPrefix + "LeanFormatString"
	)]
	[AddComponentMenu(
		Required.Scripts.LeanCommon.ComponentPathPrefix + "Format String"
	)]
	public class LeanFormatString : MonoBehaviour
	{
		[SerializeField] [Multiline] private string format = "Current Value = {0}";

		[SerializeField] private StringEvent onString;

		/// <summary>
		///     The final text will use this string formatting, where {0} is the first value, {1} is the second, etc.
		///     Formatting uses standard <b>string.Format</b> style.
		/// </summary>
		public string Format
		{
			set => format = value;
			get => format;
		}

		/// <summary>
		///     Based on the <b>Send</b> setting, this event will be invoked.
		///     String = The .
		/// </summary>
		public StringEvent OnString
		{
			get
			{
				if (onString == null) onString = new StringEvent();
				return onString;
			}
		}

		/// <summary>
		///     This method will convert the input arguments into a formatted string, and output it to the <b>OnString</b>
		///     event.
		/// </summary>
		public void SetString(string a)
		{
			SendString(
				a
			);
		}

		/// <summary>
		///     This method will convert the input arguments into a formatted string, and output it to the <b>OnString</b>
		///     event.
		/// </summary>
		public void SetString(string a, string b)
		{
			SendString(
				a,
				b
			);
		}

		/// <summary>
		///     This method will convert the input arguments into a formatted string, and output it to the <b>OnString</b>
		///     event.
		/// </summary>
		public void SetString(int a)
		{
			SendString(
				a
			);
		}

		/// <summary>
		///     This method will convert the input arguments into a formatted string, and output it to the <b>OnString</b>
		///     event.
		/// </summary>
		public void SetString(int a, int b)
		{
			SendString(
				a,
				b
			);
		}

		/// <summary>
		///     This method will convert the input arguments into a formatted string, and output it to the <b>OnString</b>
		///     event.
		/// </summary>
		public void SetString(float a)
		{
			SendString(
				a
			);
		}

		/// <summary>
		///     This method will convert the input arguments into a formatted string, and output it to the <b>OnString</b>
		///     event.
		/// </summary>
		public void SetString(float a, float b)
		{
			SendString(
				a,
				b
			);
		}

		/// <summary>
		///     This method will convert the input arguments into a formatted string, and output it to the <b>OnString</b>
		///     event.
		/// </summary>
		public void SetString(Vector2 a)
		{
			SendString(
				a
			);
		}

		/// <summary>
		///     This method will convert the input arguments into a formatted string, and output it to the <b>OnString</b>
		///     event.
		/// </summary>
		public void SetString(Vector2 a, Vector2 b)
		{
			SendString(
				a,
				b
			);
		}

		/// <summary>
		///     This method will convert the input arguments into a formatted string, and output it to the <b>OnString</b>
		///     event.
		/// </summary>
		public void SetString(Vector3 a)
		{
			SendString(
				a
			);
		}

		/// <summary>
		///     This method will convert the input arguments into a formatted string, and output it to the <b>OnString</b>
		///     event.
		/// </summary>
		public void SetString(Vector3 a, Vector3 b)
		{
			SendString(
				a,
				b
			);
		}

		/// <summary>
		///     This method will convert the input arguments into a formatted string, and output it to the <b>OnString</b>
		///     event.
		/// </summary>
		public void SetString(Vector4 a)
		{
			SendString(
				a
			);
		}

		/// <summary>
		///     This method will convert the input arguments into a formatted string, and output it to the <b>OnString</b>
		///     event.
		/// </summary>
		public void SetString(Vector4 a, Vector4 b)
		{
			SendString(
				a,
				b
			);
		}

		/// <summary>
		///     This method will convert the input arguments into a formatted string, and output it to the <b>OnString</b>
		///     event.
		/// </summary>
		public void SetString(float a, int b)
		{
			SendString(
				a,
				b
			);
		}

		/// <summary>
		///     This method will convert the input arguments into a formatted string, and output it to the <b>OnString</b>
		///     event.
		/// </summary>
		public void SetString(int a, float b)
		{
			SendString(
				a,
				b
			);
		}

		private void SendString(object a)
		{
			if (onString != null)
				onString.Invoke(
					string.Format(
						format,
						a
					)
				);
		}

		private void SendString(object a, object b)
		{
			if (onString != null)
				onString.Invoke(
					string.Format(
						format,
						a,
						b
					)
				);
		}

		[Serializable]
		public class StringEvent : UnityEvent<string>
		{
		}
	}

#if UNITY_EDITOR
	[CanEditMultipleObjects]
	[CustomEditor(
		typeof(LeanFormatString)
	)]
	public class LeanFormatString_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			LeanFormatString tgt;
			LeanFormatString[] tgts;
			GetTargets(
				out tgt,
				out tgts
			);

			BeginError(
				Any(
					tgts,
					t => string.IsNullOrEmpty(
						t.Format
					)
				)
			);
			Draw(
				"format",
				"The final text will use this string formatting, where {0} is the first value, {1} is the second, etc. Formatting uses standard <b>string.Format</b> style."
			);
			EndError();

			Separator();

			Draw(
				"onString"
			);
		}
	}

#endif
}