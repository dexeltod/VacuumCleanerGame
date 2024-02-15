using System;
using Cysharp.Threading.Tasks;

namespace Sources.ServicesInterfaces.DTO
{
	public interface IShopProgressFacade 
	{
		UniTask AddProgressPoint(string progressName, Action succeededCallback);
	}
}