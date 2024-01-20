using System;
using Cysharp.Threading.Tasks;


namespace Sources.InfrastructureInterfaces.DTO
{
	public interface IShopProgressProvider 
	{
		UniTask AddProgressPoint(string progressName, Action succeededCallback);
	}
}