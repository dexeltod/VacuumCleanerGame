using Sources.DomainInterfaces.Entities;

namespace Sources.Domain.Entities
{
	public class SceneResourceEntity : ISceneResourceEntity
	{
		public SceneResourceEntity(int id, int value)
		{
			ID = id;
			Value = value;
		}

		public int ID { get; }
		public int Value { get; }
	}
}