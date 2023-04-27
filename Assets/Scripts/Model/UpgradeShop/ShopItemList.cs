using System.Collections.Generic;
using Model.ScriptableObjects.UpgradeItems.SO;
using UnityEngine;

public class ShopItemList : MonoBehaviour
{
    [SerializeField] private List<UpgradeItemScriptableObject> _list;
    public IReadOnlyList<UpgradeItemScriptableObject> Items => _list;
}
