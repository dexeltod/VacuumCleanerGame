using System;
using System.Collections.Generic;

namespace Sources.Utils
{
	public static class StaticIdRepository
	{
		private readonly static Dictionary<string, int> _idByName = new();
		private readonly static Dictionary<string, int> _idByEnum = new();

		private static int _nextId;

		public static int AddByEnum(Enum value)
		{
			if (value == null) throw new ArgumentNullException(nameof(value));

			string name = GetNameFromEnum(value);

			if (_idByEnum.ContainsKey(name))
				throw new Exception("Name already exists in the id repository: " + name);

			_nextId++;
			_idByEnum.Add(name, _nextId);

			return _nextId;
		}

		public static int AddByName(string name)
		{
			if (_idByName.ContainsKey(name))
				throw new Exception("Name already exists in the id repository: " + name);

			_nextId++;
			_idByName.Add(name, _nextId);

			return _nextId;
		}

		public static int GetByEnum(Enum value)
		{
			if (value == null) throw new ArgumentNullException(nameof(value));

			string name = GetNameFromEnum(value);

			if (_idByEnum.TryGetValue(name, out int @enum))
				return @enum;

			throw new Exception("Could not find the id by enum " + value);
		}

		public static int GetIdByName(string name)
		{
			if (_idByName.TryGetValue(name, out int id)) return id;

			throw new Exception("Could not find  the id by name " + name);
		}

		public static int GetOrAddByEnum(Enum value) =>
			_idByEnum.TryGetValue(GetNameFromEnum(value), out int id) ? id : AddByEnum(value);

		public static int GetOrAddByName(string name) => _idByName.TryGetValue(name, out int id) ? id : AddByName(name);

		private static string GetNameFromEnum(Enum value)
		{
			string name = Enum.GetName(value.GetType(), value);

			if (name == null)
				throw new ArgumentException("Invalid enum value", nameof(value));

			return name;
		}
	}
}
