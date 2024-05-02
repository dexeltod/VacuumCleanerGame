using System;
using System.Collections.Generic;
using Sources.DomainInterfaces;
using Sources.DomainInterfaces.Models.Shop;
using Sources.DomainInterfaces.Models.Shop.Upgrades;
using UnityEngine;

namespace Sources.Domain.Progress
{
	[Serializable] public class ShopModel : IShopModel
	{
		[SerializeField] private List<UpgradeEntity> _progressEntities;

		public ShopModel(List<UpgradeEntity> progressEntities) =>
			_progressEntities = progressEntities ?? throw new ArgumentNullException(nameof(progressEntities));

		public IReadOnlyList<IUpgradeEntityReadOnly> ProgressEntities => _progressEntities;
	}
}