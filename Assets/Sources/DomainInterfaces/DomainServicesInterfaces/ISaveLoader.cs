using System;
using Cysharp.Threading.Tasks;

namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface ISaveLoader
	{
		UniTask Save(IGlobalProgress @object, Action succeededCallback = null);
		UniTask<IGlobalProgress> Load(Action succeededCallback);
		UniTask Initialize();
	}
}