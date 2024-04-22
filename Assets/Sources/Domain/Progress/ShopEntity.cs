using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Domain.Temp;
using Sources.DomainInterfaces;
using Sources.Utils;
using UnityEngine;

namespace Sources.Domain.Progress
{
	[Serializable] public class ShopEntity : IShopEntity
	{
		[SerializeField] private List<ProgressEntity> _progressEntities;

		public ShopEntity(List<ProgressEntity> progressEntities) =>
			_progressEntities = progressEntities ?? throw new ArgumentNullException(nameof(progressEntities));

		public IReadOnlyList<IProgressEntity> ProgressEntities => _progressEntities;
	}
}