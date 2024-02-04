using System;
using Cysharp.Threading.Tasks;

namespace Sources.ServicesInterfaces.DTO
{
	public interface IShopProgressProvider 
	{
		UniTask AddProgressPoint(string progressName, Action succeededCallback);
	}
}