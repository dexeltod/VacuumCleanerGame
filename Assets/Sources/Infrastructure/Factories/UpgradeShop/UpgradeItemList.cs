using Sources.Infrastructure.ScriptableObjects;
using Sources.InfrastructureInterfaces;
using Sources.PresentationInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.Factories.UpgradeShop
{
    [CreateAssetMenu(fileName = "ShopItems", menuName = "Data/Shop/Upgrade/ItemsList")]
    public class UpgradeItemList : ScriptableObject, IUpgradeItemList
    {
        [SerializeField] private UpgradeItemPrefabDataData[] _list;
        public IUpgradeItemPrefabData[] Items => _list;
    }
}
