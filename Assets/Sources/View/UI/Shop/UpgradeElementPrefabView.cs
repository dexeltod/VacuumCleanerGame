using System;
using System.Collections.Generic;
using Sources.DIService;
using Sources.InfrastructureInterfaces.Upgrade;
using Sources.PresentationInterfaces;
using Sources.ServicesInterfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.View.UI.Shop
{
	public class UpgradeElementPrefabView : MonoBehaviour, IUpgradeElementConstructable, IUpgradeInteractable,
										IColorChangeable, IDisposable
	{
		[Header("Points")] [SerializeField] private int _maxPoints = 6;

		[SerializeField] private GameObject _pointElement;
		[SerializeField] private Color      _notBoughtPointColor;
		[SerializeField] private Color      _boughtPointColor;
		[SerializeField] private Transform  _pointsContainer;

		[Header("Upgrade window")] [SerializeField]
		private TextMeshProUGUI _title;

		[SerializeField] private Image           _icon;
		[SerializeField] private Button          _buttonBuy;
		[SerializeField] private TextMeshProUGUI _description;
		[SerializeField] private TextMeshProUGUI _price;

		private readonly List<Image> _pointsColors = new();

		private IUpgradeItemData _itemData;

		private int  _boughtPoints;
		private bool _isInit;

		public string IdName => _itemData.IdName;

		public event Action<IUpgradeItemData> BuyButtonPressed;

		public IUpgradeElementConstructable Construct(IUpgradeItemData itemData, IUpgradeItemPrefab viewInfo)
		{
			if (_isInit)
				throw new InvalidOperationException($"{name} view is already constructed");

			_boughtPoints = itemData.PointLevel;
			_itemData     = itemData;

			ILocalizationService localisation = ServiceLocator.Container.Get<ILocalizationService>();

			_title.SetText(localisation.GetTranslationText(itemData.Title));
			_price.SetText(itemData.Price.ToString());
			_description.SetText(localisation.GetTranslationText(itemData.Description));
			
			_icon.sprite = viewInfo.Icon;

			InstantiatePoints();
			itemData.PriceChanged += OnPriceChanged;
			_isInit               =  true;

			return this;
		}

		public void Dispose() =>
			_itemData.PriceChanged -= OnPriceChanged;

		public void AddProgressPointColor(int count) =>
			AddPoints(count);

		private void OnEnable() =>
			_buttonBuy.onClick.AddListener(OnBuyButtonPressed);

		private void OnDisable() =>
			_buttonBuy.onClick.RemoveListener(OnBuyButtonPressed);

		private void OnDestroy() =>
			_itemData.PriceChanged -= OnPriceChanged;

		private void OnBuyButtonPressed() =>
			BuyButtonPressed?.Invoke(_itemData);

		private void OnPriceChanged(int value) =>
			_price.SetText(value.ToString());

		private void AddPoints(int count)
		{
			if (_boughtPoints + count > _maxPoints)
				return;

			_boughtPoints += count;
			
			
			for (int i = 0; i < _boughtPoints; i++)
				_pointsColors[i].color = _boughtPointColor;

			for (int i = _boughtPoints; i < _maxPoints; i++)
				_pointsColors[i].color = _notBoughtPointColor;
		}

		private void InstantiatePoints()
		{
			InstantiatePoints(0,             _boughtPoints, _boughtPointColor);
			InstantiatePoints(_boughtPoints, _maxPoints,    _notBoughtPointColor);
		}

		private void InstantiatePoints(int startIndex, int end, Color color)
		{
			if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex));

			for (int i = startIndex; i < end; i++)
			{
				GameObject @object = Instantiate(_pointElement, _pointsContainer);

				var image = @object.GetComponent<Image>();
				image.color = color;
				_pointsColors.Add(image);
			}
		}
	}
}