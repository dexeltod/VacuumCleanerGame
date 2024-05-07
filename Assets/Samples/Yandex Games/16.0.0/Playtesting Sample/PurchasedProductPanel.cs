using Agava.YandexGames;
using UnityEngine;
using UnityEngine.UI;

namespace Samples.Yandex_Games._16._0._0.Playtesting_Sample
{
    public class PurchasedProductPanel : MonoBehaviour
    {
        [SerializeField]
        private Text _purchasedProductIdText;
        [SerializeField]
        private PurchasedProductListPanel _purchasedProductListPanel;

        private PurchasedProduct _purchasedProduct;

        public PurchasedProduct PurchasedProduct
        {
            set
            {
                _purchasedProduct = value;

                _purchasedProductIdText.text = value.productID;
            }
        }

        public void OnConsumeButtonClick()
        {
            Billing.ConsumeProduct(_purchasedProduct.purchaseToken, () =>
            {
                Debug.Log($"Consumed {_purchasedProduct.productID}");

                _purchasedProductListPanel.RemovePurchasedProductPanel(this);
            });
        }
    }
}
