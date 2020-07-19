# SvgTagHelpers
ASP.NET Core Tag Helpers for inline SVG.

## SvgIconTagHelper

Add to `_ViewImports.cshtml`:
```razor
@addTagHelper *, Citizen17.SvgTagHelper
```

Then use tag `<svg-icon>` in Views.

```razor
<div>
	<svg-icon file="path/to/file.svg">
</div>
```

Result HTML:
```html
<div>
	<span>
		<svg>
			<!--SVG content-->
		</svg>
	</span>
</div>
```

Library supports absolute paths (`C:/MyIcons/file.svg`), relative to project root (`assets/icons/file.svg`) and relative to web root (`~/icons/file.svg`).

## FontAwesomeTagHelper

Uses same behavor as SvgIconTagHelper but add ability to choose icon from FontAwesome. It required installed FontAwesome package (npm/yarn prefered).

Add to `_ViewImports.cshtml`:
```razor
@addTagHelper *, Citizen17.FontAwesomeTagHelper
```

Then use tag `<font-awesome>` in Views:
```razor
<div>
	<font-awesome set="Solid" icon="bars">
</div>
```

Parameters:
 * `set` - FontAwesome set (Solid, Regular (only very small part in Free), Light (Pro only), Duotone (Pro only), Brands). By default sets to `Solid`.
 * `icon` - name of icon. WITHOUT extension.
 * `root` - (optional) path to FontAwesome package if you have specific installation. It must contains `svgs` folder.

Also some parameters can be configured uses `IServiceCollection.Configure`.
```cs
services.Configure<FontAwesomeOptions>(options =>
	{
		options.Set = FontAwesomeSet.Brands; //Change default set
		options.Root = "path/to/FontAwesome/package"; //Set default root property
	});
```

If Root not setted TagHelper tries to find installed FontAwesome package. It checks next directories:
```
"node_modules/@fortawesome/fontawesome-pro/svgs",
"node_modules/@fortawesome/fontawesome-free/svgs",
"wwwroot/lib/fortawesome/fontawesome-free/svgs",
"wwwroot/lib/fontawesome-free/svgs",
"wwwroot/lib/fontawesome/svgs",
"wwwroot/fortawesome/fontawesome-free/svgs",
"wwwroot/fontawesome-free/svgs",
"wwwroot/fontawesome/svgs"
```