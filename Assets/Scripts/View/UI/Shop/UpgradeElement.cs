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

	public event Action BuyButtonPressed;

	public void Init(UpgradeItemScriptableObject item)
	{
		_title.SetText(item.Title);
		_description.SetText(item.Description);
		_price.SetText(item.Price.ToString());
		_icon.sprite = item.Icon;
	}

	private void OnEnable() => 
		_buttonBuy.onClick.AddListener(OnBuyButtonPressed);

	private void OnDisable() => 
		_buttonBuy.onClick.RemoveListener(OnBuyButtonPressed);

	private void OnBuyButtonPressed() => 
		BuyButtonPressed?.Invoke();
}