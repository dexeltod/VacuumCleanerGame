using System;
using System.Collections.Generic;
using Sources.ControllersInterfaces;
using Sources.Presentation.Common;
using Sources.PresentationInterfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Presentation.UI.Shop
{
	public class UpgradeElementPrefabView : PresentableView<IUpgradeElementPresenter>, IUpgradeElementPrefabView
	{
		private const int StartIndex = 0;

		[SerializeField] private GameObject _pointElement;
		[SerializeField] private Color _notBoughtPointColor;
		[SerializeField] private Color _boughtPointColor;
		[SerializeField] private Transform _pointsContainer;

		[Header("Upgrade window")]
		[SerializeField]
		private TextMeshProUGUI _title;

		[SerializeField] private Image _icon;
		[SerializeField] private Button _buttonBuy;
		[SerializeField] private TextMeshProUGUI _description;
		[SerializeField] private TextMeshProUGUI _price;

		private readonly List<Image> _pointsColors = new();

		private int _boughtPoints;
		private int _id;

		private bool _isInit;
		private int _maxPoints;

		private IUpgradeElementPresenter UpgradeElementPresenter => Presenter;

		private void OnEnable() => _buttonBuy.onClick.AddListener(OnBuyButtonPressed);

		private void OnDisable() => _buttonBuy.onClick.RemoveListener(OnBuyButtonPressed);

		public event Action<int> BuyButtonPressed;

		public void Construct(
			Sprite icon,
			string title,
			string description,
			int id,
			int boughtPointsCount,
			int price,
			int maxPoints
		)
		{
			if (maxPoints < 0) throw new ArgumentOutOfRangeException(nameof(maxPoints));

			_maxPoints = maxPoints;
			_id = id;
			if (_isInit)
				throw new InvalidOperationException($"{name} view is already constructed");

			_boughtPoints = boughtPointsCount;

			_title.SetText(title);
			_price.SetText(price.ToString());
			_description.SetText(description);

			_icon.sprite = icon;

			InstantiatePoints();

			_isInit = true;
		}

		public void AddProgressPointColor(int count = 1)
		{
			IncreasePoints(count);

			SetBoughtPoints();
			SetNotBoughtPoints();
		}

		public void SetPriceText(int price) => _price.SetText(price.ToString());

		private void IncreasePoints(int count)
		{
			if (count <= 0 || _boughtPoints + count > _maxPoints)
				throw new ArgumentOutOfRangeException(nameof(count));

			_boughtPoints += count;
		}

		private void SetNotBoughtPoints()
		{
			for (int i = _boughtPoints; i < _maxPoints; i++)
				_pointsColors[i].color = _notBoughtPointColor;
		}

		private void SetBoughtPoints()
		{
			for (int i = StartIndex; i < _boughtPoints; i++)
				_pointsColors[i].color = _boughtPointColor;
		}

		private void OnBuyButtonPressed()
		{
			BuyButtonPressed!.Invoke(_id);
		}

		private void InstantiatePoints()
		{
			InstantiatePoints(StartIndex, _boughtPoints, _boughtPointColor);
			InstantiatePoints(_boughtPoints, _maxPoints, _notBoughtPointColor);
		}

		private void InstantiatePoints(int startIndex, int end, Color color)
		{
			if (startIndex < StartIndex)
				throw new ArgumentOutOfRangeException(nameof(startIndex));

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

		private Image GetImage(GameObject @object) => @object.GetComponent<Image>();
	}
}
