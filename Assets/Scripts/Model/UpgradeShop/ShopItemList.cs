using System.Collections.Generic;
using Model.ScriptableObjects.UpgradeItems.SO;
using UnityEngine;

namespace Model.UpgradeShop
{
    public class ShopItemList : MonoBehaviour
    {
        [SerializeField] private List<UpgradeItemScriptableObject> _list;
        public IReadOnlyList<UpgradeItemScriptableObject> Items => _list;
    }
}
