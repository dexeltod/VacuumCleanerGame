using System;
using Cysharp.Threading.Tasks;

namespace Sources.DomainInterfaces.DomainServicesInterfaces
{
	public interface ISaveLoader
	{
		UniTask Save(IGameProgressModel @object, Action succeededCallback);
		UniTask<IGameProgressModel> Load(Action succeededCallback);
		void Initialize();
	}
}