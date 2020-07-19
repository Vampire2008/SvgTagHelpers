using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.IO;

namespace Citizen17.SvgTagHelper
{
	/// <summary>
	/// Loads SVG content into HTML
	/// </summary>
	[HtmlTargetElement("svg-icon", Attributes = "file", TagStructure = TagStructure.WithoutEndTag)]
	public class SvgIconTagHelper : TagHelper
	{
		protected readonly IWebHostEnvironment _environment;

		public SvgIconTagHelper(IWebHostEnvironment environment)
		{
			_environment = environment;
		}

		/// <summary>
		/// Path to SVG file
		/// </summary>
		public string File { get; set; }

		/// <summary>
		/// Sets tag that wraps svg. By default it span.
		/// </summary>
		public string Tag { get; set; } = "span";

		protected string GetPath(string path)
		{
			return Path.IsPathRooted(path)
				? path
				: path.StartsWith("~")
					? Path.Combine(_environment.WebRootPath, path.Substring(2))
					: Path.Combine(_environment.ContentRootPath, path);
		}

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			if (string.IsNullOrWhiteSpace(File))
			{
				throw new ArgumentNullException(nameof(File));
			}
			var path = GetPath(File);
			output.TagName = Tag;
			output.TagMode = TagMode.StartTagAndEndTag;
			output.Content.SetHtmlContent(System.IO.File.ReadAllText(path));
		}
	}
}
