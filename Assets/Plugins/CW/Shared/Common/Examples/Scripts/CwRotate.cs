using Plugins.CW.Shared.Common.Required.Scripts;
using UnityEditor;
using UnityEngine;

namespace Plugins.CW.Shared.Common.Examples.Scripts
{
	/// <summary>This component rotates the current <b>Transform</b>.</summary>
	[HelpURL(
		CwShared.HelpUrlPrefix + "CwRotate"
	)]
	[AddComponentMenu(
		CwShared.ComponentMenuPrefix + "Rotate"
	)]
	public class CwRotate : MonoBehaviour
	{
		/// <summary>The speed of the rotation in degrees per second.</summary>
		public Vector3 AngularVelocity
		{
			set { angularVelocity = value; }
			get { return angularVelocity; }
		}

		[SerializeField] private Vector3 angularVelocity = Vector3.up;

		/// <summary>The rotation space.</summary>
		public Space RelativeTo
		{
			set { relativeTo = value; }
			get { return relativeTo; }
		}

		[SerializeField] private Space relativeTo;

		protected virtual void Update()
		{
			transform.Rotate(
				angularVelocity * Time.deltaTime,
				relativeTo
			);
		}
	}

#if UNITY_EDITOR
	[CanEditMultipleObjects]
	[CustomEditor(
		typeof(CwRotate)
	)]
	public class CwRotate_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			CwRotate tgt;
			CwRotate[] tgts;
			GetTargets(
				out tgt,
				out tgts
			);

			BeginError(
				Any(
					tgts,
					t => t.AngularVelocity.magnitude == 0.0f
				)
			);
			Draw(
				"angularVelocity",
				"The speed of the rotation in degrees per second."
			);
			EndError();
			Draw(
				"relativeTo",
				"The rotation space."
			);
		}
	}

#endif
}