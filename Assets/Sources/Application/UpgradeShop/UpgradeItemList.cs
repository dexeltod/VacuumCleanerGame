using System.Collections.Generic;
using Application.ScriptableObjects.UpgradeItems.SO;
using UnityEngine;

namespace Application.UpgradeShop
{
    [CreateAssetMenu(fileName = "ShopItems", menuName = "Data/Shop/Upgrade/ItemsList")]
    public class UpgradeItemList : ScriptableObject
    {
        [SerializeField] private List<UpgradeItemScriptableObject> _list;
        public IReadOnlyList<UpgradeItemScriptableObject> Items => _list;
    }
}
