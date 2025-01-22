using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Plugins.CW.LeanLocalization.Required.Scripts
{
	/// <summary>This contains the translated value for the current language, and other associated data.</summary>
	public class LeanTranslation
	{
		private static bool buffering;

		private readonly static StringBuilder current = new();

		private readonly static StringBuilder buffer = new();

		private readonly static List<LeanToken> tokens = new();

		/// <summary>
		///     The data of this translation (e.g. string or Object).
		///     NOTE: This is a System.Object, so you must correctly cast it back before use.
		/// </summary>
		public object Data;

		[SerializeField] private readonly string name;

		/// <summary>If Data has been filled with data for the primary language, then this will be set to true.</summary>
		public bool Primary;

		public LeanTranslation(string newName) =>
			name = newName;

		/// <summary>The name of this translation.</summary>
		public string Name => name;

		/// <summary>
		///     This stores a list of all LeanSource instances that are currently managing the current value of this translation in
		///     the current language.
		///     NOTE: If this is empty then no LeanSource of this name is localized for the current language.
		/// </summary>
		public List<Entry> Entries { get; } = new();

		public void Clear()
		{
			Data = null;
			Primary = false;

			Entries.Clear();
		}

		/// <summary>
		///     This returns Text with all tokens substituted using the LeanLocalization.Tokens list.
		///     NOTE: If you want local tokens to work, then specify the localTokenRoot GameObject.
		/// </summary>
		public static string FormatText(string rawText,
			string currentText = null,
			ILocalizationHandler handler = null,
			GameObject localTokenRoot = null)
		{
			if (string.IsNullOrEmpty(
				    currentText
			    ))
				currentText = rawText;

			if (rawText != null)
			{
				current.Length = 0;
				buffer.Length = 0;
				tokens.Clear();

				for (var i = 0; i < rawText.Length; i++)
				{
					char rawChar = rawText[i];

					if (rawChar == '{')
					{
						if (buffering)
						{
							buffering = false;

							buffer.Length = 0;
						}
						else
						{
							buffering = true;
						}
					}
					else if (rawChar == '}')
					{
						if (buffering)
						{
							if (buffer.Length > 0)
							{
								var token = default(LeanToken);

								// Try and replace local tokens first
								if (buffer.Length > 0 &&
								    localTokenRoot != null &&
								    LeanLocalToken.TryGetLocalToken(
									    localTokenRoot,
									    buffer.ToString(),
									    ref token
								    )) // TODO: Avoid ToString here?
								{
									current.Append(
										token.Value
									);

									tokens.Add(
										token
									);
								}
								// Try and replace global tokens second
								else if (buffer.Length > 0 &&
								         LeanLocalization.CurrentTokens.TryGetValue(
									         buffer.ToString(),
									         out token
								         )) // TODO: Avoid ToString here?
								{
									current.Append(
										token.Value
									);

									tokens.Add(
										token
									);
								}
								// If none found, leave the token text as it was
								else
								{
									current.Append(
										'{'
									).Append(
										buffer
									).Append(
										'}'
									);
								}

								buffer.Length = 0;
							}

							buffering = false;
						}
					}
					else
					{
						if (buffering)
							buffer.Append(
								rawChar
							);
						else
							current.Append(
								rawChar
							);
					}
				}

				if (Match(
					    currentText,
					    current
				    ) ==
				    false)
				{
					if (handler != null)
					{
						handler.UnregisterAll();

						for (int i = tokens.Count - 1; i >= 0; i--)
						{
							LeanToken token = tokens[i];

							token.Register(
								handler
							);

							handler.Register(
								token
							);
						}
					}

					return current.ToString();
				}
			}

			return currentText;
		}

		public int LanguageCount(string language)
		{
			var total = 0;

			for (int i = Entries.Count - 1; i >= 0; i--)
				if (Entries[i].Language == language)
					total += 1;

			return total;
		}

		public void Register(string language, Object owner)
		{
			var entry = new Entry();

			entry.Language = language;
			entry.Owner = owner;

			Entries.Add(
				entry
			);
		}

		private static bool Match(string a, StringBuilder b)
		{
			if (a == null && b.Length > 0) return false;

			if (a.Length != b.Length) return false;

			for (var i = 0; i < a.Length; i++)
				if (a[i] != b[i])
					return false;

			return true;
		}

		public struct Entry
		{
			public string Language;

			public Object Owner;
		}
	}
}