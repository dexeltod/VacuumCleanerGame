using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sources.ApplicationServicesInterfaces;
using Sources.DomainInterfaces;
using Unity.Services.CloudSave;

namespace Sources.Services.DomainServices
{
	public class UnityCloudSaveLoader : ICloudSave
	{
		private const string GameProgressKey = "GameProgress";

		public async UniTask Save(string json) =>
			await CloudSaveService.Instance.Data.ForceSaveAsync(
				new Dictionary<string, object>
				{
					{ GameProgressKey, json }
				}
			);

		public async UniTask<string> Load()
		{
			try
			{
				Dictionary<string, string> keyAndJsonSaves = await CloudSaveService
					.Instance
					.Data
					.LoadAsync
					(
						new HashSet<string>
						{
							GameProgressKey
						}
					);

				string jsonSave = keyAndJsonSaves.Values.LastOrDefault();

				return jsonSave;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		public async UniTask DeleteSaves(IGlobalProgress globalProgress) { }

		public UniTask Initialize() =>
			throw new NotImplementedException();
	}
}