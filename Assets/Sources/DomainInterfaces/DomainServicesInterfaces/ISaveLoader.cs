using System;
using Cysharp.Threading.Tasks;

namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface ISaveLoader
	{
		UniTask Initialize();
		UniTask<IGlobalProgress> Load(Action succeededCallback);
		UniTask Save(IGlobalProgress @object, Action succeededCallback = null);
	}
}