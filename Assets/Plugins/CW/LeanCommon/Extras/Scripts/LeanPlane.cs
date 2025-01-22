using Plugins.CW.Shared.Common.Required.Scripts;
using UnityEditor;
using UnityEngine;

namespace Plugins.CW.LeanCommon.Extras.Scripts
{
	/// <summary>
	///     This component stores information about a 3D plane. By default this plane lays on the XY axis, or faces the Z
	///     axis.
	/// </summary>
	[HelpURL(
		Required.Scripts.LeanCommon.HelpUrlPrefix + "LeanPlane"
	)]
	[AddComponentMenu(
		Required.Scripts.LeanCommon.ComponentPathPrefix + "Plane"
	)]
	public class LeanPlane : MonoBehaviour
	{
		/// <summary>Should the plane be clamped on the x axis?</summary>
		public bool ClampX;

		public float MinX;

		public float MaxX;

		/// <summary>Should the plane be clamped on the y axis?</summary>
		public bool ClampY;

		public float MinY;

		public float MaxY;

		/// <summary>The distance between each position snap on the x axis.</summary>
		public float SnapX;

		/// <summary>The distance between each position snap on the x axis.</summary>
		public float SnapY;

#if UNITY_EDITOR
		protected virtual void OnDrawGizmosSelected()
		{
			Gizmos.matrix = transform.localToWorldMatrix;

			float x1 = MinX;
			float x2 = MaxX;
			float y1 = MinY;
			float y2 = MaxY;

			if (ClampX == false)
			{
				x1 = -1000.0f;
				x2 = 1000.0f;
			}

			if (ClampY == false)
			{
				y1 = -1000.0f;
				y2 = 1000.0f;
			}

			if (ClampX == false && ClampY == false)
			{
				Gizmos.DrawLine(
					new Vector3(
						x1,
						0.0f
					),
					new Vector3(
						x2,
						0.0f
					)
				);
				Gizmos.DrawLine(
					new Vector3(
						0.0f,
						y1
					),
					new Vector3(
						0.0f,
						y2
					)
				);
			}
			else
			{
				Gizmos.DrawLine(
					new Vector3(
						x1,
						y1
					),
					new Vector3(
						x2,
						y1
					)
				);
				Gizmos.DrawLine(
					new Vector3(
						x1,
						y2
					),
					new Vector3(
						x2,
						y2
					)
				);

				Gizmos.DrawLine(
					new Vector3(
						x1,
						y1
					),
					new Vector3(
						x1,
						y2
					)
				);
				Gizmos.DrawLine(
					new Vector3(
						x2,
						y1
					),
					new Vector3(
						x2,
						y2
					)
				);
			}
		}
#endif

		public Vector3 GetClosest(Vector3 position, float offset = 0.0f)
		{
			// Transform point to plane space
			Vector3 point = transform.InverseTransformPoint(
				position
			);

			// Clamp values?
			if (ClampX)
				point.x = Mathf.Clamp(
					point.x,
					MinX,
					MaxX
				);

			if (ClampY)
				point.y = Mathf.Clamp(
					point.y,
					MinY,
					MaxY
				);

			// Snap values?
			if (SnapX != 0.0f)
				point.x = Mathf.Round(
					          point.x / SnapX
				          ) *
				          SnapX;

			if (SnapY != 0.0f)
				point.y = Mathf.Round(
					          point.y / SnapY
				          ) *
				          SnapY;

			// Reset Z to plane
			point.z = 0.0f;

			// Transform back into world space
			return transform.TransformPoint(
				       point
			       ) +
			       transform.forward * offset;
		}

		public bool TryRaycast(Ray ray, ref Vector3 hit, float offset = 0.0f, bool getClosest = true)
		{
			Vector3 normal = transform.forward;
			Vector3 point = transform.position + normal * offset;
			var distance = default(float);

			if (RayToPlane(
				    point,
				    normal,
				    ray,
				    ref distance
			    ))
			{
				hit = ray.GetPoint(
					distance
				);

				if (getClosest)
					hit = GetClosest(
						hit,
						offset
					);

				return true;
			}

			return false;
		}

		public Vector3 GetClosest(Ray ray, float offset = 0.0f)
		{
			Vector3 normal = transform.forward;
			Vector3 point = transform.position + normal * offset;
			var distance = default(float);

			if (RayToPlane(
				    point,
				    normal,
				    ray,
				    ref distance
			    ))
				return GetClosest(
					ray.GetPoint(
						distance
					),
					offset
				);

			return point;
		}

		private static bool RayToPlane(Vector3 point, Vector3 normal, Ray ray, ref float distance)
		{
			float b = Vector3.Dot(
				ray.direction,
				normal
			);

			if (Mathf.Approximately(
				    b,
				    0.0f
			    ))
				return false;

			float d = -Vector3.Dot(
				normal,
				point
			);
			float a = -Vector3.Dot(
				          ray.origin,
				          normal
			          ) -
			          d;

			distance = a / b;

			return distance > 0.0f;
		}
	}

#if UNITY_EDITOR
	[CanEditMultipleObjects]
	[CustomEditor(
		typeof(LeanPlane)
	)]
	public class LeanPlane_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			LeanPlane tgt;
			LeanPlane[] tgts;
			GetTargets(
				out tgt,
				out tgts
			);

			Draw(
				"ClampX",
				"Should the plane be clamped on the x axis?"
			);

			if (Any(
				    tgts,
				    t => t.ClampX
			    ))
			{
				BeginIndent();
				Draw(
					"MinX",
					"",
					"Min"
				);
				Draw(
					"MaxX",
					"",
					"Max"
				);
				EndIndent();

				Separator();
			}

			Draw(
				"ClampY",
				"Should the plane be clamped on the y axis?"
			);

			if (Any(
				    tgts,
				    t => t.ClampX
			    ))
			{
				BeginIndent();
				Draw(
					"MinY",
					"",
					"Min"
				);
				Draw(
					"MaxY",
					"",
					"Max"
				);
				EndIndent();

				Separator();
			}

			Draw(
				"SnapX",
				"The distance between each position snap on the x axis."
			);
			Draw(
				"SnapY",
				"The distance between each position snap on the y axis."
			);
		}
	}

#endif
}