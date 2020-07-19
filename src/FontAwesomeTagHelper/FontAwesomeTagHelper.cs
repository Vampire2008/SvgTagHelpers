using Citizen17.SvgTagHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Citizen17.FontAwesomeTagHelper
{
	/// <summary>
	/// Place inline icon from FontAwesome package.
	/// </summary>
	[HtmlTargetElement("font-awesome", TagStructure = TagStructure.WithoutEndTag)]
	public class FontAwesomeTagHelper : SvgIconTagHelper
	{
		private static string FoundedRoot;

		private readonly IReadOnlyCollection<string> _roots = new ReadOnlyCollection<string>(new string[]
		{
			"node_modules/@fortawesome/fontawesome-pro/svgs",
			"node_modules/@fortawesome/fontawesome-free/svgs",
			"wwwroot/lib/fortawesome/fontawesome-free/svgs",
			"wwwroot/lib/fontawesome-free/svgs",
			"wwwroot/lib/fontawesome/svgs",
			"wwwroot/fortawesome/fontawesome-free/svgs",
			"wwwroot/fontawesome-free/svgs",
			"wwwroot/fontawesome/svgs"
		});

		private readonly FontAwesomeOptions _options;
		private readonly ILogger _logger;
		/// <summary>
		/// Not used. Inherited from SvgTagHelper
		/// </summary>
		[Obsolete]
		public new string File { get; set; }

		/// <summary>
		/// FontAwesome set
		/// </summary>
		public FontAwesomeSet Set { get; set; }

		/// <summary>
		/// Name of icon. Without extension
		/// </summary>
		public string Icon { get; set; }

		/// <summary>
		/// Optional. Path to FontAwesome package if you have specific installation
		/// </summary>
		public string Root { get; set; }

		public FontAwesomeTagHelper(IWebHostEnvironment environment,
			IOptionsSnapshot<FontAwesomeOptions> options,
			ILogger<FontAwesomeTagHelper> logger) : base(environment)
		{
			_options = options.Value;
			Set = _options.Set;
			_logger = logger;
		}

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			if (string.IsNullOrWhiteSpace(Icon))
			{
				throw new ArgumentNullException(nameof(Icon));
			}

			_logger.LogDebug("Root property: {0}", Root);
			_logger.LogDebug("Options.Root property: {0}", _options.Root);
			_logger.LogDebug("FoundedRoot static propery: {0}", FoundedRoot);
			Root ??= _options.Root ?? FoundedRoot;
			if (string.IsNullOrWhiteSpace(Root))
			{
				_logger.LogDebug("Root is empty, try to find FontAwesome.");
				foreach (var path in _roots)
				{
					_logger.LogDebug("Checking path: {0}", path);
					var fullPath = Path.Combine(_environment.ContentRootPath, path);
					_logger.LogDebug("Full path: {0}", fullPath);
					if (Directory.Exists(fullPath))
					{
						_logger.LogDebug("FontAwesome founded");
						FoundedRoot = Root = path;
						break;
					}
				}
				if (string.IsNullOrWhiteSpace(Root))
				{
					throw new ArgumentException("Countn't find Font Awesome folder with svgs folder. Please, specify Font Awesome directory in Root parameter or in configuration");
				}
			}
			else
			{
				if (!Root.Contains("svgs"))
					Root = Path.Combine(Root, "svgs");
				Root = GetPath(Root);
				if (!Directory.Exists(Root))
					throw new ArgumentException("Wrong Font Awesome root folder");
			}
			base.File = Path.Combine(Root, Path.Combine(Set.ToString().ToLowerInvariant(), $"{Icon.ToLowerInvariant()}.svg"));
			_logger.LogDebug("SVG file: {0}", base.File);
			base.Process(context, output);
		}
	}
}
