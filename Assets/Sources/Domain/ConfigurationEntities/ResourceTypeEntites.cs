using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Domain.ConfigurationEntities
{
	[Serializable]
	public class ResourceTypeEntities
	{
		[SerializeField] public List<CurrencyTypeEntity> CurrencyTypes;
	}

	[Serializable]
	public class CurrencyTypeEntity
	{
		[SerializeField] public int Id;
		[SerializeField] public string Name;
	}
}
