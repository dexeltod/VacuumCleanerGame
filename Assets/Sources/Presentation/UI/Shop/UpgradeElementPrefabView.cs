using System;
using System.Collections.Generic;
using Sources.Controllers;
using Sources.Presentation.Common;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces.Upgrade;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Presentation.UI.Shop
{
	public class UpgradeElementPrefabView : PresentableView<IUpgradeElementPresenter>,
		IColorChangeable, IDisposable, IUpgradeElementPrefabView
	{
		private const int StartIndex = 0;
		
		[Header("Points")] [SerializeField] private int _maxPoints = 6;

		[SerializeField] private GameObject _pointElement;
		[SerializeField] private Color _notBoughtPointColor;
		[SerializeField] private Color _boughtPointColor;
		[SerializeField] private Transform _pointsContainer;

		[Header("Upgrade window")] [SerializeField]
		private TextMeshProUGUI _title;

		[SerializeField] private Image _icon;
		[SerializeField] private Button _buttonBuy;
		[SerializeField] private TextMeshProUGUI _description;
		[SerializeField] private TextMeshProUGUI _price;

		private readonly List<Image> _pointsColors = new();

		private IUpgradeItemData _itemData;

		private int _boughtPoints;
		private bool _isInit;
		private IItemChangeable _itemChangeable;
		private string _idName;

		public string IdName => _itemData.IdName;

		public void Construct(
			IUpgradeElementPresenter presenter,
			IItemChangeable itemChangeable,
			IUpgradeItemData itemData,
			IUpgradeItemPrefab viewInfo,
			string title,
			string description,
			string idName
		)
		{
			if (_isInit)
				throw new InvalidOperationException($"{name} view is already constructed");

			base.Construct(presenter);
			_idName = idName;
			_boughtPoints = itemData.PointLevel;
			_itemData = itemData;
			_itemChangeable = itemChangeable;

			_title.SetText(title);
			_price.SetText(itemData.Price.ToString());
			_description.SetText(description);

			_icon.sprite = viewInfo.Icon;

			InstantiatePoints();
			itemChangeable.PriceChanged += OnPriceChanged;
			_isInit = true;
		}

		public void Dispose() =>
			_itemChangeable.PriceChanged -= OnPriceChanged;

		public void AddProgressPointColor(int count)
		{
			if (_boughtPoints + count > _maxPoints)
				return;

			_boughtPoints += count;

			for (int i = StartIndex; i < _boughtPoints; i++)
				_pointsColors[i].color = _boughtPointColor;

			for (int i = _boughtPoints; i < _maxPoints; i++)
				_pointsColors[i].color = _notBoughtPointColor;
		}

		private void OnEnable()
		{
			Presenter.Enable();
			_buttonBuy.onClick.AddListener(OnBuyButtonPressed);
		}

		private void OnDisable()
		{
			Presenter.Disable();
			_buttonBuy.onClick.RemoveListener(OnBuyButtonPressed);
		}

		private void OnDestroy() =>
			_itemChangeable.PriceChanged -= OnPriceChanged;

		private void OnBuyButtonPressed() =>
			Presenter.Upgrade(_idName);

		private void OnPriceChanged(int value) =>
			_price.SetText(value.ToString());

		private void InstantiatePoints()
		{
			InstantiatePoints(StartIndex, _boughtPoints, _boughtPointColor);
			InstantiatePoints(_boughtPoints, _maxPoints, _notBoughtPointColor);
		}

		private void InstantiatePoints(int startIndex, int end, Color color)
		{
			if (startIndex < StartIndex) throw new ArgumentOutOfRangeException(nameof(startIndex));

			for (int i = startIndex; i < end; i++)
			{
				GameObject @object = Instantiate(_pointElement, _pointsContainer);
				AddColor(color, @object);
			}
		}

		private void AddColor(Color color, GameObject @object)
		{
			Image image = GetImage(@object);
			image.color = color;
			_pointsColors.Add(image);
		}

		private Image GetImage(GameObject @object) =>
			@object.GetComponent<Image>();
	}
}