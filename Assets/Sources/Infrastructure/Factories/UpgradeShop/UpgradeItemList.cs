using Sources.Infrastructure.ScriptableObjects;
using Sources.InfrastructureInterfaces;
using UnityEngine;

namespace Sources.Infrastructure.Factories.UpgradeShop
{
    [CreateAssetMenu(fileName = "ShopItems", menuName = "Data/Shop/Upgrade/ItemsList")]
    public class UpgradeItemList : ScriptableObject, IUpgradeItemList<UpgradeItemViewData>
    {
        [SerializeField] private UpgradeItemViewData[] _list;
        public UpgradeItemViewData[] Items => _list;
    }
}
