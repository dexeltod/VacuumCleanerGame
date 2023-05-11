using System;
using Model.ScriptableObjects.UpgradeItems.SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeElement : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _title;
	[SerializeField] private TextMeshProUGUI _description;
	[SerializeField] private TextMeshProUGUI _price;
	[SerializeField] private Image _icon;
	[SerializeField] private Button _buttonBuy;

	private UpgradeItemScriptableObject.Upgrade _upgradeType;
	public event Action<UpgradeItemScriptableObject.Upgrade> BuyButtonPressed;

	public UpgradeElement Construct(UpgradeItemScriptableObject item)
	{
		_upgradeType = item.UpgradeType;
		_title.SetText(item.Title);
		_description.SetText(item.Description);
		_price.SetText(item.Price.ToString());
		_icon.sprite = item.Icon;
		return this;
	}

	private void OnEnable() =>
		_buttonBuy.onClick.AddListener(OnBuyButtonPressed);

	private void OnDisable() =>
		_buttonBuy.onClick.RemoveListener(OnBuyButtonPressed);

	private void OnBuyButtonPressed() =>
		BuyButtonPressed?.Invoke(_upgradeType);
}