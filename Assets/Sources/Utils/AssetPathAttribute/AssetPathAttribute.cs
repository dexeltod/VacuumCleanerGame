using System;
using UnityEngine;

namespace Sources.Utils.AssetPathAttribute
{
	public partial class AssetPath
	{
		/// <summary>
		///     We limit this attributes to fields and only allow one. Should
		///     only be applied to string types.
		/// </summary>
		[AttributeUsage(AttributeTargets.Field)]
		public class Attribute : PropertyAttribute
		{
			/// <summary>
			///     Creates the default instance of AssetPathAttribute
			/// </summary>
			public Attribute(Type type)
			{
				this.type = type;
				pathType = Types.Project;
			}

			/// <summary>
			///     Gets the type of asset path this attribute is watching.
			/// </summary>
			public Types pathType { get; }

			/// <summary>
			///     Gets the type of asset this attribute is expecting.
			/// </summary>
			public Type type { get; }

			public string SuperProperty =>
				/* whole pile of work done here */
				"Complex string example";

			public void Evulate()
			{
				string value = SuperProperty;
			}
		}
	}
}