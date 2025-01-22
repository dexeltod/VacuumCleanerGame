using System;
using System.Collections.Generic;
using Plugins.CW.Shared.Common.Required.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Plugins.CW.LeanCommon.Extras.Scripts
{
	/// <summary>This is the base class for all object selectors.</summary>
	[HelpURL(
		Required.Scripts.LeanCommon.HelpUrlPrefix + "LeanSelect"
	)]
	[AddComponentMenu(
		Required.Scripts.LeanCommon.ComponentPathPrefix + "Select"
	)]
	public class LeanSelect : MonoBehaviour
	{
		public enum LimitType
		{
			Unlimited,
			StopAtMax,
			DeselectFirst
		}

		public enum ReselectType
		{
			KeepSelected,
			Deselect,
			DeselectAndSelect,
			SelectAgain
		}

		public static LinkedList<LeanSelect> Instances = new();

		[SerializeField] private bool deselectWithNothing;

		[SerializeField] private LimitType limit;

		[SerializeField] private int maxSelectables = 5;

		[SerializeField] private ReselectType reselect = ReselectType.SelectAgain;

		[SerializeField] protected List<LeanSelectable> selectables;

		[SerializeField] private LeanSelectableEvent onSelected;

		[SerializeField] private LeanSelectableEvent onDeselected;

		[SerializeField] private UnityEvent onNothing;
		[NonSerialized] private LinkedListNode<LeanSelect> instancesNode;

		/// <summary>
		///     If you attempt to select a point that has no objects underneath, should all currently selected objects be
		///     deselected?
		/// </summary>
		public bool DeselectWithNothing
		{
			set => deselectWithNothing = value;
			get => deselectWithNothing;
		}

		/// <summary>
		///     If you have selected the maximum number of objects, what should happen?
		///     Unlimited = Always allow selection.
		///     StopAtMax = Allow selection up to the <b>MaxSelectables</b> count, then do nothing.
		///     DeselectFirst = Always allow selection, but deselect the first object when the <b>MaxSelectables</b> count is
		///     reached.
		/// </summary>
		public LimitType Limit
		{
			set => limit = value;
			get => limit;
		}

		/// <summary>
		///     The maximum number of selectables that can be selected at the same time.
		///     0 = Unlimited.
		/// </summary>
		public int MaxSelectables
		{
			set => maxSelectables = value;
			get => maxSelectables;
		}

		/// <summary>If you select an already selected selectable, what should happen?</summary>
		public ReselectType Reselect
		{
			set => reselect = value;
			get => reselect;
		}

		/// <summary>This stores all objects selected by this component.</summary>
		public List<LeanSelectable> Selectables
		{
			get
			{
				if (selectables == null) selectables = new List<LeanSelectable>();
				return selectables;
			}
		}

		/// <summary>This is invoked when an object is selected.</summary>
		public LeanSelectableEvent OnSelected
		{
			get
			{
				if (onSelected == null) onSelected = new LeanSelectableEvent();
				return onSelected;
			}
		}

		/// <summary>This is invoked when an object is deselected.</summary>
		public LeanSelectableEvent OnDeselected
		{
			get
			{
				if (onDeselected == null) onDeselected = new LeanSelectableEvent();
				return onDeselected;
			}
		}

		/// <summary>This is invoked when you try to select, but nothing is found.</summary>
		public UnityEvent OnNothing
		{
			get
			{
				if (onNothing == null) onNothing = new UnityEvent();
				return onNothing;
			}
		}

		protected virtual void OnEnable()
		{
			instancesNode = Instances.AddLast(
				this
			);
		}

		protected virtual void OnDisable()
		{
			Instances.Remove(
				instancesNode
			);
			instancesNode = null;
		}

		protected virtual void OnDestroy()
		{
			DeselectAll();
		}

		public static event Action<LeanSelect, LeanSelectable> OnAnySelected;

		public static event Action<LeanSelect, LeanSelectable> OnAnyDeselected;

		public bool IsSelected(LeanSelectable selectable) =>
			selectables != null &&
			selectables.Contains(
				selectable
			);

		/// <summary>
		///     This will select the specified object and add it to this component's <b>Selectables</b> list, if it isn't
		///     already there.
		/// </summary>
		public void Select(LeanSelectable selectable)
		{
			TrySelect(
				selectable
			);
		}

		/// <summary>This remove the specified object from this component's <b>Selectables</b> list if present, and deselect it.</summary>
		public void Deselect(LeanSelectable selectable)
		{
			if (selectable != null && selectables != null)
				TryDeselect(
					selectable
				);
		}

		protected bool TrySelect(LeanSelectable selectable)
		{
			if (CwHelper.Enabled(
				    selectable
			    ))
			{
				if (TryReselect(
					    selectable
				    ))
				{
					if (Selectables.Contains(
						    selectable
					    ) ==
					    false) // NOTE: Property
						switch (limit)
						{
							case LimitType.Unlimited:
							{
							}
								break;

							case LimitType.StopAtMax:
							{
								if (selectables.Count >= maxSelectables) return false;
							}
								break;

							case LimitType.DeselectFirst:
							{
								if (selectables.Count > 0 && selectables.Count >= maxSelectables)
									TryDeselect(
										selectables[0]
									);
							}
								break;
						}

					selectables.Add(
						selectable
					);

					if (onSelected != null)
						onSelected.Invoke(
							selectable
						);

					if (OnAnySelected != null)
						OnAnySelected.Invoke(
							this,
							selectable
						);

					selectable.InvokeOnSelected(
						this
					);

					return true;
				}
			}
			// Nothing was selected?
			else
			{
				if (onNothing != null) onNothing.Invoke();

				if (deselectWithNothing) DeselectAll();
			}

			return false;
		}

		private bool TryReselect(LeanSelectable selectable)
		{
			switch (reselect)
			{
				case ReselectType.KeepSelected:
				{
					if (Selectables.Contains(
						    selectable
					    ) ==
					    false) // NOTE: Property
						return true;
				}
					break;

				case ReselectType.Deselect:
				{
					if (Selectables.Contains(
						    selectable
					    ) ==
					    false) // NOTE: Property
						return true;

					Deselect(
						selectable
					);
				}
					break;

				case ReselectType.DeselectAndSelect:
				{
					if (Selectables.Contains(
						    selectable
					    )) // NOTE: Property
						Deselect(
							selectable
						);
				}
					return true;

				case ReselectType.SelectAgain:
				{
				}
					return true;
			}

			return false;
		}

		protected bool TryDeselect(LeanSelectable selectable)
		{
			if (selectables != null)
			{
				int index = selectables.IndexOf(
					selectable
				);

				if (index >= 0)
					return TryDeselect(
						index
					);
			}

			return false;
		}

		protected bool TryDeselect(int index)
		{
			var success = false;

			if (selectables != null && index >= 0 && index < selectables.Count)
			{
				LeanSelectable selectable = selectables[index];

				selectables.RemoveAt(
					index
				);

				if (selectable != null)
				{
					selectable.InvokeOnDeslected(
						this
					);

					if (onDeselected != null)
						onDeselected.Invoke(
							selectable
						);

					if (OnAnyDeselected != null)
						OnAnyDeselected.Invoke(
							this,
							selectable
						);
				}

				success = true;
			}

			return success;
		}

		/// <summary>This will deselect all objects that were selected by this component.</summary>
		[ContextMenu(
			"Deselect All"
		)]
		public void DeselectAll()
		{
			if (selectables != null)
				while (selectables.Count > 0)
				{
					int index = selectables.Count - 1;
					LeanSelectable selectable = selectables[index];

					selectables.RemoveAt(
						index
					);

					selectable.InvokeOnDeslected(
						this
					);
				}
		}

		/// <summary>
		///     This will deselect objects in chronological order until the selected object count reaches the specified
		///     amount.
		/// </summary>
		public void Cull(int maxCount)
		{
			if (selectables != null)
				while (selectables.Count > 0 && selectables.Count > maxCount)
				{
					LeanSelectable selectable = selectables[0];

					selectables.RemoveAt(
						0
					);

					if (selectable != null)
						if (selectable != null)
							Deselect(
								selectable
							);
				}
		}

		[Serializable]
		public class LeanSelectableEvent : UnityEvent<LeanSelectable>
		{
		}
	}

#if UNITY_EDITOR
	[CanEditMultipleObjects]
	[CustomEditor(
		typeof(LeanSelect)
	)]
	public class LeanSelect_Editor : CwEditor
	{
		[NonSerialized] private LeanSelect tgt;
		[NonSerialized] private LeanSelect[] tgts;

		protected override void OnInspector()
		{
			GetTargets(
				out tgt,
				out tgts
			);

			Draw(
				"deselectWithNothing",
				"If you attempt to select a point that has no objects underneath, should all currently selected objects be deselected?"
			);
			Draw(
				"limit",
				"If you have selected the maximum number of objects, what should happen?\n\nUnlimited = Always allow selection.\n\nStopAtMax = Allow selection up to the <b>MaxSelectables</b> count, then do nothing.\n\nDeselectFirst = Always allow selection, but deselect the first object when the <b>MaxSelectables</b> count is reached."
			);

			if (Any(
				    tgts,
				    t => t.Limit != LeanSelect.LimitType.Unlimited
			    ))
			{
				BeginIndent();
				Draw(
					"maxSelectables",
					"The maximum number of selectables that can be selected at the same time.\n\n0 = Unlimited."
				);
				EndIndent();
			}

			Draw(
				"reselect",
				"If you select an already selected selectable, what should happen?"
			);

			Separator();

			var select = (LeanSelectable)EditorGUILayout.ObjectField(
				new GUIContent(
					"Select",
					"Drop a selectable object here to select it."
				),
				null,
				typeof(LeanSelectable),
				true
			);
			var deselect = (LeanSelectable)EditorGUILayout.ObjectField(
				new GUIContent(
					"Deselect",
					"Drop a selectable object here to deselect it."
				),
				null,
				typeof(LeanSelectable),
				true
			);

			BeginDisabled();
			Draw(
				"selectables",
				"This stores all objects selected by this component."
			);
			EndDisabled();

			Separator();

			bool showUnusedEvents = DrawFoldout(
				"Show Unused Events",
				"Show all events?"
			);

			DrawEvents(
				showUnusedEvents
			);

			if (select != null)
				Each(
					tgts,
					t => t.Select(
						select
					),
					true
				);

			if (deselect != null)
				Each(
					tgts,
					t => t.Deselect(
						deselect
					),
					true
				);
		}

		protected virtual void DrawEvents(bool showUnusedEvents)
		{
			if (Any(
				    tgts,
				    t => t.OnSelected.GetPersistentEventCount() > 0
			    ) ||
			    showUnusedEvents)
				Draw(
					"onSelected"
				);

			if (Any(
				    tgts,
				    t => t.OnDeselected.GetPersistentEventCount() > 0
			    ) ||
			    showUnusedEvents)
				Draw(
					"onDeselected"
				);

			if (Any(
				    tgts,
				    t => t.OnNothing.GetPersistentEventCount() > 0
			    ) ||
			    showUnusedEvents)
				Draw(
					"onNothing"
				);
		}
	}

#endif
}