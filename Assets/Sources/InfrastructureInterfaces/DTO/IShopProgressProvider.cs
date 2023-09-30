using System;
using Cysharp.Threading.Tasks;
using Sources.DIService;

namespace Sources.InfrastructureInterfaces.DTO
{
	public interface IShopProgressProvider : IService
	{
		UniTask AddProgressPoint(string progressName, Action succeededCallback);
	}
}