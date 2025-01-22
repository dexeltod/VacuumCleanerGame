using UnityEditor;
using UnityEngine;

namespace Plugins.CW.Shared.Common.Extras.Scripts
{
	/// <summary>
	///     This is the base class for all components that are created as children of another component, allowing them to
	///     be more easily managed.
	/// </summary>
	public abstract class CwChild : MonoBehaviour
	{
		protected virtual void Start()
		{
			//DestroyGameObjectIfInvalid();
		}

		[ContextMenu(
			"Destroy GameObject If Invalid All"
		)]
		public void DestroyGameObjectIfInvalidAll()
		{
			if (transform.parent != null)
				foreach (CwChild siblings in transform.parent.GetComponentsInChildren<CwChild>())
					siblings.DestroyGameObjectIfInvalid();
		}

		[ContextMenu(
			"Destroy GameObject If Invalid"
		)]
		public void DestroyGameObjectIfInvalid()
		{
			IHasChildren parent = GetParent();

			if (parent == null ||
			    parent.HasChild(
				    this
			    ) ==
			    false)
			{
#if UNITY_EDITOR
				Undo.DestroyObjectImmediate(
					gameObject
				);
#else
				DestroyImmediate(gameObject);
#endif
			}
		}

		protected abstract IHasChildren GetParent();

		public interface IHasChildren
		{
			bool HasChild(CwChild child);
		}
	}
}