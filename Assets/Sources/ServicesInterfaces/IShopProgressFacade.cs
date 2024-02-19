using System;
using Cysharp.Threading.Tasks;

namespace Sources.ServicesInterfaces
{
	public interface IShopProgressFacade 
	{
		UniTask AddProgressPoint(string progressName, Action succeededCallback);
	}
}