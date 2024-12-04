using System;
using System.Linq;
using Sources.InfrastructureInterfaces.Common.Providers;

namespace Sources.Infrastructure.Common.Provider
{
	public abstract class Provider<TImplementation> : IProvider<TImplementation> where TImplementation : class
	{
		private TImplementation _implementation;

		private Type[] _contracts;

		public TImplementation Self
		{
			get
			{
				if (_implementation == null)
					throw new NullReferenceException(
						$"{nameof(_implementation)} {typeof(TImplementation)} {nameof(TImplementation)}  is null"
					);

				return _implementation;
			}
			set => _implementation = value;
		}

		public TI GetContract<TI>() where TI : class
		{
			if (_contracts == null || !_contracts.Contains(typeof(TI)))
				throw new ArgumentNullException(nameof(TI), $"Contract {typeof(TI)} not found");

			Type interfaceType = typeof(TI);

			// Check if _implementation implements the interface directly
			if (interfaceType.IsInstanceOfType(_implementation))
				return _implementation as TI;

			// If not, check if it implements the interface through its interfaces
			Type implementedInterface = _contracts.FirstOrDefault(contract => interfaceType.IsAssignableFrom(contract));

			if (implementedInterface != null)
				return _implementation as TI;

			throw new ArgumentNullException(nameof(TI), $"Contract {typeof(TI)} not found");
		}

#region Registration

		public virtual TImplementation Register<TI>(TImplementation instance)
		{
			Unregister();
			_implementation = instance;

			var a = instance.GetType().GetInterfaces();

			_contracts = instance.GetType().GetInterfaces();
			return _implementation;
		}

		public virtual TImplementation Register(TImplementation instance) =>
			_implementation = instance;

#endregion

		public virtual void Unregister()
		{
			_implementation = null;
			_contracts = null;
		}
	}
}