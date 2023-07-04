using System;
using System.Collections.Generic;
using Application.ScriptableObjects.UpgradeItems.SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View.UI.Shop
{
	public class UpgradeElementView : MonoBehaviour
	{
		[Header("Points")] [SerializeField] private int _maxPoints = 6;

		[SerializeField] private Color _notBoughtPointColor;
		[SerializeField] private Color _boughtPointColor;
		[SerializeField] private Transform _pointsContainer;
		[SerializeField] private GameObject _pointElement;

		[Header("Upgrade window")] [SerializeField]
		private TextMeshProUGUI _title;

		[SerializeField] private TextMeshProUGUI _description;
		[SerializeField] private TextMeshProUGUI _price;
		[SerializeField] private Image _icon;
		[SerializeField] private Button _buttonBuy;

		private readonly List<Image> _pointsColors = new();

		private UpgradeItemScriptableObject.Upgrade _upgradeType;
		private UpgradeItemScriptableObject _item;
		private int _boughtPoints;
		private bool _isInit;

		public UpgradeItemScriptableObject ItemData { get; private set; }
		public event Action<UpgradeItemScriptableObject.Upgrade> BuyButtonPressed;

		public UpgradeElementView Construct(UpgradeItemScriptableObject item, int boughtPoints)
		{
			if (_isInit)
				throw new InvalidOperationException("Item view is already constructed");

			_item = item;
			_boughtPoints = boughtPoints;
			_upgradeType = item.UpgradeType;

			ItemData = item;
			_title.SetText(item.Title);

			_price.SetText(item.Price.ToString());

			_description.SetText(item.Description);
			_icon.sprite = item.Icon;

			InstantiatePoints();
			item.PriceChanged += OnPriceChanged;
			_isInit = true;

			return this;
		}

		public void AddProgressPointColor(int count) =>
			ChangePointsColor(count);

		private void OnEnable() =>
			_buttonBuy.onClick.AddListener(OnBuyButtonPressed);

		private void OnDisable() =>
			_buttonBuy.onClick.RemoveListener(OnBuyButtonPressed);

		private void OnDestroy() => 
			_item.PriceChanged -= OnPriceChanged;

		private void OnBuyButtonPressed() =>
			BuyButtonPressed?.Invoke(_upgradeType);

		private void OnPriceChanged(int value) =>
			_price.SetText(value.ToString());

		private void ChangePointsColor(int count)
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
			InstantiatePoints(0, _boughtPoints, _boughtPointColor);
			InstantiatePoints(_boughtPoints, _maxPoints, _notBoughtPointColor);
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