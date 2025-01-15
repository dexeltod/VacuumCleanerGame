using System;
using System.Collections.Generic;
using Sources.DomainInterfaces.Models.Shop;
using Sources.DomainInterfaces.Models.Shop.Upgrades;
using UnityEngine;

namespace Sources.Domain.Progress
{
	[Serializable]
	public class ShopModel : IShopModel
	{
		[SerializeField] private List<StatUpgradeEntity> _progressEntities;

		public ShopModel(List<StatUpgradeEntity> progressEntities) =>
			_progressEntities = progressEntities ?? throw new ArgumentNullException(nameof(progressEntities));

		public IReadOnlyList<IStatUpgradeEntityReadOnly> ProgressEntities => _progressEntities;
	}
}