using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugins.SerializeInterfaces.Editor.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Plugins.SerializeInterfaces.Editor
{
	internal class ObjectSelectorWindow : EditorWindow
	{
		private readonly static ItemInfo _nullItem = new() { InstanceID = null, Label = "None" };
		private List<ItemInfo> _allItems;
		private Tab _assetsTab;
		private ItemInfo _currentItem;
		private Label _detailsIndexLabel;
		private Label _detailsLabel;
		private Label _detailsTypeLabel;
		private SerializedProperty _editingProperty;
		private ObjectSelectorFilter _filter;
		private List<ItemInfo> _filteredItems;
		private ListView _listView;
		private Tab _sceneTab;
		private ToolbarSearchField _searchbox;
		private string _searchText;

		private Action<Object> _selectionChangedCallback;
		private Action<Object, bool> _selectorClosedCallback;
		private bool _showSceneObjects = true;
		private int _undoGroup;
		private bool _userCanceled;

		public static ObjectSelectorWindow Instance { get; private set; }

		public bool initialized { get; private set; }

		public string SearchText
		{
			get => _searchText;
			set
			{
				_searchText = value;
				FilterItems();
			}
		}

		private void OnDisable()
		{
			_selectorClosedCallback?.Invoke(
				GetCurrentObject(),
				_userCanceled
			);
			if (_userCanceled)
				Undo.RevertAllDownToGroup(
					_undoGroup
				);
			else
				Undo.CollapseUndoOperations(
					_undoGroup
				);
			Instance = null;
		}

		public static void Show(SerializedProperty property,
			Action<Object> onSelectionChanged,
			Action<Object, bool> onSelectorClosed,
			ObjectSelectorFilter filter)
		{
			if (Instance == null)
				Instance = CreateInstance<ObjectSelectorWindow>();
			Instance._editingProperty = property;
			Instance._selectionChangedCallback = onSelectionChanged;
			Instance._selectorClosedCallback = onSelectorClosed;
			Instance._filter = filter;
			Instance.Init();
			Instance.ShowAuxWindow();
			//Instance.Show();
		}

		public void SetSearchFilter(string query)
		{
			_searchbox.value = query;
		}

		private void Init()
		{
			InitData();
			InitVisualElements();
			BindVisualElements();
			FinishInit();
		}

		private void InitData()
		{
			_undoGroup = Undo.GetCurrentGroup();
			_searchText = "";
			_allItems = new List<ItemInfo>();
			_filteredItems = new List<ItemInfo>();

			_showSceneObjects = true;
			Object target = _editingProperty.objectReferenceValue;
			if (target != null)
				_showSceneObjects = !AssetDatabase.Contains(
					target
				);

			PopulateItems();
			FilterItems();
		}

		private void InitVisualElements()
		{
			var styleSheet =
				AssetDatabase.LoadAssetAtPath<StyleSheet>(
					"Assets/SerializeInterfaces/Assets/USS/ObjectSelectorWindow.uss"
				);
			rootVisualElement.styleSheets.Add(
				styleSheet
			);

			_searchbox = new ToolbarSearchField();
			_searchbox.RegisterValueChangedCallback(
				SearchFilterChanged
			);
			rootVisualElement.Add(
				_searchbox
			);

			var tabContainer = new VisualElement();
			tabContainer.style.flexDirection = FlexDirection.Row;
			_assetsTab = new Tab(
				"Assets"
			);
			_sceneTab = new Tab(
				"Scene"
			);
			tabContainer.Add(
				_assetsTab
			);
			tabContainer.Add(
				_sceneTab
			);
			rootVisualElement.Add(
				tabContainer
			);

			_listView = new ListView(
				_filteredItems,
				16,
				MakeItem,
				BindItem
			);
			_listView.onSelectionChange += ItemSelectionChanged;
			_listView.onItemsChosen += ItemsChosen;
			rootVisualElement.Add(
				_listView
			);

			_detailsLabel = new Label();
			_detailsTypeLabel = new Label();
			_detailsIndexLabel = new Label();

			var details = new VisualElement();
			details.AddToClassList(
				"details"
			);
			details.Add(
				_detailsLabel
			);
			details.Add(
				_detailsIndexLabel
			);
			details.Add(
				_detailsTypeLabel
			);
			rootVisualElement.Add(
				details
			);
		}

		private void BindVisualElements()
		{
			Tab activeTab = _showSceneObjects ? _sceneTab : _assetsTab;
			activeTab.SetValueWithoutNotify(
				true
			);

			var toggleGroup = new ToggleGroup();
			toggleGroup.RegisterToggle(
				_assetsTab
			);
			toggleGroup.RegisterToggle(
				_sceneTab
			);
			toggleGroup.OnToggleChanged += HandleGroupChanged;

			if (GetIndexOfEditingPropertyValue(
				    out int index
			    ))
				_listView.selectedIndex = index;
		}

		private void FinishInit()
		{
			EditorApplication.delayCall += () =>
			{
				_listView.Focus();
				initialized = true;
			};
		}

		private bool GetIndexOfEditingPropertyValue(out int index)
		{
			index = -1;
			Object targetObject = _editingProperty.objectReferenceValue;

			if (targetObject)
			{
				int instanceID = targetObject.GetInstanceID();
				index = _filteredItems.FindIndex(
					x => x.InstanceID == instanceID
				);
			}

			return index >= 0;
		}

		private bool GetIndexOfCurrentItem(out int index)
		{
			index = -1;
			if (_currentItem != null)
				index = _filteredItems.FindIndex(
					0,
					x => x.InstanceID == _currentItem.InstanceID
				);
			return index >= 0;
		}

		private void HandleGroupChanged(object sender, Toggle toggle)
		{
			if (_showSceneObjects && toggle == _sceneTab) return;

			_showSceneObjects = !_showSceneObjects;
			PopulateItems();
			FilterItems();
			var list = new List<int>();
			if (GetIndexOfCurrentItem(
				    out int index
			    ))
				list.Add(
					index
				);
			_listView.SetSelectionWithoutNotify(
				list
			);
			_listView.Focus();
		}

		private void PopulateItems()
		{
			_allItems.Clear();
			_filteredItems.Clear();
			if (_showSceneObjects)
				_allItems.AddRange(
					FetchAllComponents()
				);
			else
				_allItems.AddRange(
					FetchAllAssets()
				);
			_allItems.Sort(
				(item, other) => item.Label.CompareTo(
					other.Label
				)
			);
		}

		private void SearchFilterChanged(ChangeEvent<string> evt)
		{
			SearchText = evt.newValue;
		}

		private void FilterItems()
		{
			_filteredItems.Clear();
			_filteredItems.Add(
				_nullItem
			);
			_filteredItems.AddRange(
				_allItems.Where(
					item =>
						string.IsNullOrEmpty(
							SearchText
						) ||
						item.Label.IndexOf(
							SearchText,
							StringComparison.InvariantCultureIgnoreCase
						) >=
						0
				)
			);

			_listView?.Rebuild();
		}

		private void BindItem(VisualElement listItem, int index)
		{
			if (index < 0 || index >= _filteredItems.Count)
				return;

			var label = listItem.Q<Label>();
			if (label != null)
				label.text = _filteredItems[index].Label;
			var image = listItem.Q<Image>();
			image.image = _filteredItems[index].Icon;
		}

		private static VisualElement MakeItem()
		{
			var ve = new VisualElement();
			var image = new Image();
			var label = new Label();
			ve.Add(
				image
			);
			ve.Add(
				label
			);

			ve.AddToClassList(
				"list-item"
			);
			label.AddToClassList(
				"list-item__text"
			);
			image.AddToClassList(
				"list-item__icon"
			);

			return ve;
		}

		private void ItemSelectionChanged(IEnumerable<object> selectedItems)
		{
			_currentItem = selectedItems.FirstOrDefault() as ItemInfo;
			UpdateDetails();
			_selectionChangedCallback?.Invoke(
				GetCurrentObject()
			);
		}

		private void ItemsChosen(IEnumerable<object> selectedItems)
		{
			_currentItem = selectedItems.FirstOrDefault() as ItemInfo;
			_userCanceled = false;
			Close();
		}

		private void UpdateDetails()
		{
			GetText(
				_currentItem,
				out string infoText,
				out string indexText,
				out string typeText
			);

			void SetText(Label label, string text)
			{
				label.text = string.IsNullOrEmpty(
					text
				)
					? ""
					: text;
			}

			SetText(
				_detailsLabel,
				infoText
			);
			SetText(
				_detailsIndexLabel,
				indexText
			);
			SetText(
				_detailsTypeLabel,
				typeText
			);
		}

		private static void GetText(ItemInfo itemInfo, out string text, out string indexText, out string typeText)
		{
			text = null;
			indexText = null;
			typeText = null;

			if (itemInfo == null) return;

			if (itemInfo.InstanceID == null)
			{
				text = itemInfo.Label;
				return;
			}

			Object obj = EditorUtility.InstanceIDToObject(
				(int)itemInfo.InstanceID
			);

			if (AssetDatabase.Contains(
				    obj
			    ))
			{
				text = AssetDatabase.GetAssetPath(
					obj
				);
			}
			else
			{
				Transform transform = obj is GameObject go ? go.transform : (obj as Component).transform;
				int compIndex = Array.IndexOf(
					transform.gameObject.GetComponents(
						typeof(Component)
					),
					obj
				);
				text = $"{GetTransformPath(transform)}";
				indexText = $"[{compIndex}]";
			}

			typeText = $"({obj.GetType().Name})";
		}

		private static string GetTransformPath(Transform transform)
		{
			var sb = new StringBuilder();
			sb.Append(
				transform.name
			);

			while (transform.parent != null)
			{
				sb.Insert(
					0,
					transform.parent.name + "/"
				);
				transform = transform.parent;
			}

			return sb.ToString();
		}

		private IEnumerable<ItemInfo> FetchAllAssets()
		{
			var property = new HierarchyProperty(
				HierarchyType.Assets,
				false
			);
			property.SetSearchFilter(
				_filter.AssetSearchFilter,
				0
			);

			while (property.Next(
				       null
			       ))
				yield return new ItemInfo { Icon = property.icon, InstanceID = property.instanceID, Label = property.name };
		}

		private IEnumerable<ItemInfo> FetchAllComponents()
		{
			var property = new HierarchyProperty(
				HierarchyType.GameObjects,
				false
			);

			while (property.Next(
				       null
			       ))
			{
				var go = property.pptrValue as GameObject;
				if (go == null) continue;

				if (CheckFilter(
					    go
				    ))
					yield return new ItemInfo { Icon = property.icon, InstanceID = property.instanceID, Label = property.name };

				foreach (Component comp in go.GetComponents(
					         typeof(Component)
				         ))
					if (CheckFilter(
						    comp
					    ))
						yield return new ItemInfo
						{
							Icon = EditorGUIUtility.ObjectContent(
								comp,
								comp.GetType()
							).image,
							InstanceID = comp.GetInstanceID(),
							Label = property.name
						};
			}
		}

		private bool CheckFilter(Object obj)
		{
			bool? matchFilterConstraint = _filter.SceneFilterCallback?.Invoke(
				obj
			);
			return !matchFilterConstraint.HasValue || matchFilterConstraint.Value;
		}

		private Object GetCurrentObject()
		{
			if (_currentItem == null || _currentItem.InstanceID == null) return null;

			return EditorUtility.InstanceIDToObject(
				(int)_currentItem.InstanceID
			);
		}

		public class ItemInfo
		{
			public Texture Icon;
			public int? InstanceID;
			public string Label;
		}
	}

	public class ObjectSelectorFilter
	{
		public string AssetSearchFilter;
		public Func<Object, bool> SceneFilterCallback;

		public ObjectSelectorFilter() : this(
			"",
			x => true
		)
		{
		}

		public ObjectSelectorFilter(string assetSearchFilter, Func<Object, bool> sceneFilterCallback)
		{
			AssetSearchFilter = assetSearchFilter;
			SceneFilterCallback = sceneFilterCallback;
		}
	}
}