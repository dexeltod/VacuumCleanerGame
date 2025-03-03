﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace Plugins.LinkerGenerator
{
	public class FileGenerator
	{
		private readonly LinkerSettings _settings;

		public FileGenerator(LinkerSettings settings) => _settings = settings;

		public void Generate()
		{
			string assetsDir = Application.dataPath;

			IOrderedEnumerable<string> assembliesToPreserve = Enumerable.Empty<string>()
				.Concat(
					GetDllAssemblyNames(
						assetsDir
					)
				)
				.Concat(
					GetAsmdefAssemblyNames()
				)
				.Concat(
					GetRspAssemblyNames(
						assetsDir
					)
				)
				.Distinct()
				.Where(
					NotIgnoredByName
				)
				.Where(
					NotIgnoredByPattern
				)
				.OrderBy(
					s => s
				);

			string linkXmlFilePath = Path.Combine(
				assetsDir,
				_settings.FolderPath,
				"link.xml"
			);

			Directory.CreateDirectory(
				Path.GetDirectoryName(
					linkXmlFilePath
				) ??
				throw new InvalidOperationException(
					$"No directory in file name {linkXmlFilePath}"
				)
			);

			using (FileStream fileStream = File.Open(
				       linkXmlFilePath,
				       FileMode.Create
			       ))
			using (var streamWriter = new StreamWriter(
				       fileStream
			       ))
			{
				Enumerable.Empty<string>()
					.Concat(
						"<linker>"
					)
					.Concat(
						string.Empty
					)
					.Concat(
						assembliesToPreserve.Select(
							assemblyName => $"    <assembly fullname=\"{assemblyName}\" preserve=\"all\" />"
						)
					)
					.Concat(
						string.Empty
					)
					.Concat(
						"</linker>"
					)
					.ForEach(
						streamWriter.WriteLine
					);
			}
		}

		[MenuItem(
			"Window / Linker / Generate link.xml"
		)]
		public static void GenerateLinkXml()
		{
			var settings = LinkerSettings.GetOrCreateSettings();

			new FileGenerator(
				settings
			).Generate();
		}

		private IEnumerable<string> GetAsmdefAssemblyNames() => !_settings.AddAsmdefs
			? Array.Empty<string>()
			: CompilationPipeline.GetAssemblies(
					AssembliesType.PlayerWithoutTestAssemblies
				)
				.Select(
					a => a.name
				)
				.Distinct();

		private IEnumerable<string> GetDllAssemblyNames(string assetsDir) => !_settings.AddDlls
			? Array.Empty<string>()
			: Directory.EnumerateFiles(
					assetsDir,
					"*.dll",
					SearchOption.AllDirectories
				)
				.Distinct()
				.Select(
					Path.GetFileNameWithoutExtension
				);

		private IEnumerable<string> GetRspAssemblyNames(string assetsDir) => !_settings.AddRsps
			? Array.Empty<string>()
			: Directory.EnumerateFiles(
					assetsDir,
					"*.rsp",
					SearchOption.AllDirectories
				)
				.Distinct()
				.SelectMany(
					File.ReadAllLines
				)
				.Where(
					l => l.StartsWith(
						"-r:"
					)
				)
				.Distinct()
				.Select(
					l => l.Remove(
						0,
						3
					)
				)
				.Select(
					Path.GetFileNameWithoutExtension
				);

		private bool NotIgnoredByName(string assemblyName) => !_settings.AssembliesToIgnore.Contains(
			assemblyName
		);

		private bool NotIgnoredByPattern(string assemblyName) => !_settings
			.AssemblyPatternsToIgnore
			.Any(
				p =>
				{
					try
					{
						return new Regex(
							p
						).IsMatch(
							assemblyName
						);
					}
					catch (Exception e)
					{
						Debug.LogError(
							$"Invalid regex: {e.Message}"
						);
						return false;
					}
				}
			);
	}
}