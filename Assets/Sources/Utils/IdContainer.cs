namespace Sources.Utils
{
	// public class IdRepository
	// {
	// 	private readonly Dictionary<string, int> _idByName = new();
	// 	private int _nextId;
	//
	// 	public int AddByEnum(Enum value)
	// 	{
	// 		if (value == null) throw new ArgumentNullException(nameof(value));
	//
	// 		string name = Enum.GetName(value.GetType(), value);
	// 		if (name == null)
	// 			throw new ArgumentException("Invalid enum value", nameof(value));
	//
	// 		AddByName(name);
	//
	// 		return _nextId;
	// 	}
	//
	// 	public int AddByName(string name)
	// 	{
	// 		if (_idByName.ContainsKey(name))
	// 			throw new Exception("Name already exists in the id repository: " + name);
	//
	// 		_nextId++;
	// 		_idByName.Add(name, _nextId);
	//
	// 		return _nextId;
	// 	}
	//
	// 	public int GetByEnum(Enum value)
	// 	{
	// 		if (value == null) throw new ArgumentNullException(nameof(value));
	//
	// 		string name = Enum.GetName(value.GetType(), value);
	// 		if (name == null)
	// 			throw new ArgumentException("Invalid enum value", nameof(value));
	//
	// 		return _idByName[name];
	// 	}
	//
	// 	public int GetIdByName(string name)
	// 	{
	// 		if (_idByName.TryGetValue(name, out int id)) return id;
	//
	// 		throw new Exception("Could not find  the id by name " + name);
	// 	}
	//
	// 	public int GetOrAddByName(string name) => _idByName.TryGetValue(name, out int id) ? id : AddByName(name);
	//
	// 	public bool TryGetIdByName(string name, out int id) => _idByName.TryGetValue(name, out id);
	// }
}
