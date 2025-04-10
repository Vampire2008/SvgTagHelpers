using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Citizen17.SvgTagHelper;

/// <summary>
/// Loads SVG content into HTML
/// </summary>
[HtmlTargetElement("svg-icon", TagStructure = TagStructure.WithoutEndTag)]
public class SvgIconTagHelper : TagHelper
{
    protected readonly IWebHostEnvironment Environment;

    public SvgIconTagHelper(IWebHostEnvironment environment)
    {
        Environment = environment;
    }

    /// <summary>
    /// Path to SVG file
    /// </summary>
    public string? File { get; set; }

    /// <summary>
    /// Sets tag that wraps svg. By default, it is span.
    /// </summary>
    public string Tag { get; set; } = "span";

    protected string GetPath(string path)
    {
        return Path.IsPathRooted(path)
            ? path
            : path.StartsWith("~")
                ? Path.Combine(Environment.WebRootPath, path.Substring(2))
                : Path.Combine(Environment.ContentRootPath, path);
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