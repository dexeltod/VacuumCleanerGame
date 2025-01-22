using Plugins.CW.Shared.Common.Required.Scripts;
using UnityEditor;
using UnityEngine;

namespace Plugins.CW.Shared.Common.Extras.Scripts
{
	/// <summary>This makes the current <b>Transform</b> follow the <b>Target</b> Transform as if it were a child.</summary>
	[ExecuteInEditMode]
	[HelpURL(
		CwShared.HelpUrlPrefix + "CwFollow"
	)]
	[AddComponentMenu(
		CwShared.ComponentMenuPrefix + "Follow"
	)]
	public class CwFollow : MonoBehaviour
	{
		public enum FollowType
		{
			TargetTransform,
			MainCamera
		}

		public enum UpdateType
		{
			Update,
			LateUpdate
		}

		[SerializeField] private FollowType follow;

		[SerializeField] private Transform target;

		[SerializeField] private float damping = -1.0f;

		[SerializeField] private bool rotate = true;

		[SerializeField] private bool ignoreZ;

		[SerializeField] private UpdateType followIn = UpdateType.LateUpdate;

		[SerializeField] private Vector3 localPosition;

		[SerializeField] private Vector3 localRotation;

		/// <summary>What should this component follow?</summary>
		public FollowType Follow
		{
			set => follow = value;
			get => follow;
		}

		/// <summary>The transform that will be followed.</summary>
		public Transform Target
		{
			set => target = value;
			get => target;
		}

		/// <summary>
		///     How quickly this Transform follows the target.
		///     -1 = instant.
		/// </summary>
		public float Damping
		{
			set => damping = value;
			get => damping;
		}

		/// <summary>Follow the target's rotation too?</summary>
		public bool Rotate
		{
			set => rotate = value;
			get => rotate;
		}

		/// <summary>Ignore Z axis for 2D?</summary>
		public bool IgnoreZ
		{
			set => ignoreZ = value;
			get => ignoreZ;
		}

		/// <summary>Where in the game loop should this component update?</summary>
		public UpdateType FollowIn
		{
			set => followIn = value;
			get => followIn;
		}

		/// <summary>This allows you to specify a positional offset relative to the <b>Target</b>.</summary>
		public Vector3 LocalPosition
		{
			set => localPosition = value;
			get => localPosition;
		}

		/// <summary>This allows you to specify a rotational offset relative to the <b>Target</b>.</summary>
		public Vector3 LocalRotation
		{
			set => localRotation = value;
			get => localRotation;
		}

		protected virtual void Update()
		{
			if (followIn == UpdateType.Update) UpdatePosition();
		}

		protected virtual void LateUpdate()
		{
			if (followIn == UpdateType.LateUpdate) UpdatePosition();
		}

		/// <summary>This method will update the follow position now.</summary>
		[ContextMenu(
			"UpdatePosition"
		)]
		public void UpdatePosition()
		{
			Transform finalTarget = target;

			if (follow == FollowType.MainCamera)
			{
				Camera mainCamera = Camera.main;

				if (mainCamera != null) finalTarget = mainCamera.transform;
			}

			if (finalTarget != null)
			{
				Vector3 currentPosition = transform.position;
				Vector3 targetPosition = finalTarget.TransformPoint(
					localPosition
				);
				float factor = CwHelper.DampenFactor(
					damping,
					Time.deltaTime
				);

				if (ignoreZ) targetPosition.z = currentPosition.z;

				transform.position = Vector3.Lerp(
					currentPosition,
					targetPosition,
					factor
				);

				if (rotate)
				{
					Quaternion targetRotation = finalTarget.rotation *
					                            Quaternion.Euler(
						                            localRotation
					                            );

					transform.rotation = Quaternion.Slerp(
						transform.rotation,
						targetRotation,
						factor
					);
				}
			}
		}
	}

#if UNITY_EDITOR
	[CanEditMultipleObjects]
	[CustomEditor(
		typeof(CwFollow)
	)]
	public class CwFollow_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			CwFollow tgt;
			CwFollow[] tgts;
			GetTargets(
				out tgt,
				out tgts
			);

			Draw(
				"follow",
				"What should this component follow?"
			);

			if (Any(
				    tgts,
				    t => t.Follow == CwFollow.FollowType.TargetTransform
			    ))
			{
				BeginIndent();
				BeginError(
					Any(
						tgts,
						t => t.Target == null
					)
				);
				Draw(
					"target",
					"The transform that will be followed."
				);
				EndError();
				EndIndent();
			}

			Draw(
				"damping",
				"How quickly this Transform follows the target.\n\n-1 = instant."
			);
			Draw(
				"rotate",
				"Follow the target's rotation too?"
			);
			Draw(
				"ignoreZ",
				"Ignore Z axis for 2D?"
			);
			Draw(
				"followIn",
				"Where in the game loop should this component update?"
			);

			Separator();

			Draw(
				"localPosition",
				"This allows you to specify a positional offset relative to the Target transform."
			);
			Draw(
				"localRotation",
				"This allows you to specify a rotational offset relative to the Target transform."
			);
		}
	}

#endif
}