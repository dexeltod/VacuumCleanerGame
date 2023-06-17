using System.Collections.Generic;
using Model.ScriptableObjects.UpgradeItems.SO;
using UnityEngine;

namespace Model.UpgradeShop
{
    [CreateAssetMenu(fileName = "ShopItems", menuName = "Data/Shop/Upgrade/ItemsList")]
    public class ShopItemList : ScriptableObject
    {
        [SerializeField] private List<UpgradeItemScriptableObject> _list;
        public IReadOnlyList<UpgradeItemScriptableObject> Items => _list;
    }
}
