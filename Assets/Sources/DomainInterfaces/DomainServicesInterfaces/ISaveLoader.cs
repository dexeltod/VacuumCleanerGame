using System;
using Cysharp.Threading.Tasks;

namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface ISaveLoader
	{
		UniTask Save(IGameProgressProvider @object, Action succeededCallback);
		UniTask<IGameProgressProvider> Load(Action succeededCallback);
		UniTask ClearSaves(IGameProgressProvider gameProgressProvider, Action succeededCallback);
		UniTask Initialize();
	}
}