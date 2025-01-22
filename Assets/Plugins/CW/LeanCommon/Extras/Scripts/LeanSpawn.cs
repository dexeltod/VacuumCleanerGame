using Plugins.CW.Shared.Common.Required.Scripts;
using UnityEditor;
using UnityEngine;

namespace Plugins.CW.LeanCommon.Extras.Scripts
{
	/// <summary>
	///     This component allows you to spawn a prefab at the specified world point.
	///     NOTE: For this component to work you must manually call the <b>Spawn</b> method from somewhere.
	/// </summary>
	[HelpURL(
		Required.Scripts.LeanCommon.HelpUrlPrefix + "LeanSpawn"
	)]
	[AddComponentMenu(
		Required.Scripts.LeanCommon.ComponentPathPrefix + "Spawn"
	)]
	public class LeanSpawn : MonoBehaviour
	{
		public enum SourceType
		{
			ThisTransform,
			Prefab
		}

		[SerializeField] private Transform prefab;

		[SerializeField] private SourceType defaultPosition;

		[SerializeField] private SourceType defaultRotation;

		/// <summary>The prefab that this component can spawn.</summary>
		public Transform Prefab
		{
			set => prefab = value;
			get => prefab;
		}

		/// <summary>If you call <b>Spawn()</b>, where should the position come from?</summary>
		public SourceType DefaultPosition
		{
			set => defaultPosition = value;
			get => defaultPosition;
		}

		/// <summary>If you call <b>Spawn()</b>, where should the rotation come from?</summary>
		public SourceType DefaultRotation
		{
			set => defaultRotation = value;
			get => defaultRotation;
		}

		/// <summary>This will spawn <b>Prefab</b> at the current <b>Transform.position</b>.</summary>
		public void Spawn()
		{
			if (prefab != null)
			{
				Vector3 position = defaultPosition == SourceType.Prefab ? prefab.position : transform.position;
				Quaternion rotation = defaultRotation == SourceType.Prefab ? prefab.rotation : transform.rotation;
				Transform clone = Instantiate(
					prefab,
					position,
					rotation
				);

				clone.gameObject.SetActive(
					true
				);
			}
		}

		/// <summary>This will spawn <b>Prefab</b> at the specified position in world space.</summary>
		public void Spawn(Vector3 position)
		{
			if (prefab != null)
			{
				Quaternion rotation = defaultRotation == SourceType.Prefab ? prefab.rotation : transform.rotation;
				Transform clone = Instantiate(
					prefab,
					position,
					rotation
				);

				clone.gameObject.SetActive(
					true
				);
			}
		}
	}

#if UNITY_EDITOR
	[CanEditMultipleObjects]
	[CustomEditor(
		typeof(LeanSpawn)
	)]
	public class LeanSpawn_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			LeanSpawn tgt;
			LeanSpawn[] tgts;
			GetTargets(
				out tgt,
				out tgts
			);

			BeginError(
				Any(
					tgts,
					t => t.Prefab == null
				)
			);
			Draw(
				"prefab",
				"The prefab that this component can spawn."
			);
			EndError();
			Draw(
				"defaultPosition",
				"If you call Spawn(), where should the position come from?"
			);
			Draw(
				"defaultRotation",
				"If you call Spawn(), where should the rotation come from?"
			);
		}
	}

#endif
}