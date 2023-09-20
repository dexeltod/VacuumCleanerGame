using System;
using Codice.Client.GameUI.Explorer;
using Sources.DIService;

namespace Sources.InfrastructureInterfaces.DTO
{
	public interface IPlayerProgressProvider : IService
	{
		void SetProgress(string progressName);
	}
}