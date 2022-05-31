
[<p align='center'><img src='https://support.crowdin.com/assets/logos/crowdin-dark-symbol.png' data-canonical-src='https://support.crowdin.com/assets/logos/crowdin-dark-symbol.png' width='200' height='200' align='center'/></p>](https://crowdin.com)

# Crowdin Xamarin Forms SDK
The Crowdin Xamarin SDK delivers all new translations from Crowdin project to the application immediately.

### Features

+ Load static strings from the bundled RESX files
    <br>
    Usable as fallback before CDN strings loaded
+ Load remote strings from Crowdin Distributions CDN
+ Built-in caching mechanism (enabled by default, can be disabled)
+ Network usage restrictions (All, only Wi-Fi or Cellular, Forbidden)

### Requirements

* .NET Standard 2.1 support
* Xamarin Forms 5.0 or newer

### Installation

Install via NuGet:

```
// Package Manager
Install-Package Crowdin.Xamarin.Forms -Version 0.1.0

// .Net CLI
dotnet add package Crowdin.Xamarin.Forms --version 0.1.0

// Package Reference
<PackageReference Include="Crowdin.Xamarin.Forms" Version="0.1.0" />

// Paket CLI
paket add Crowdin.Xamarin.Forms --version 0.1.0
```

---

### Quick start

For applications with localization in XML resource files (RESX)

1) Add Crowdin Distribution Hash before any modules initialization

```C#
DynamicResourcesLoader.GlobalOptions.DistributionHash = "{distribution_id}";
```

2) Load static strings from app resource files to use as fallback

```C#
DynamicResourcesLoader.LoadStaticStrings(Translations.ResourceManager, Current.Resources);
```

The first argument is the source - ResourceManager of the generated class from resources group (Translations.resx and descendants).
The second argument is the destination - ResourceDictionary where to place loaded strings:

* Global: in `Application.Current.Resources`
* Per-view: in `ContentPage.Resources`

3) Load strings from Crowdin Distributions CDN 

```C#
string langCode = DynamicResourcesLoader.CurrentCulture.TwoLetterISOLanguageName;
DynamicResourcesLoader.LoadCrowdinStrings($"Translations.{langCode}.resx", Current.Resources);
```

The property `CurrentCulture` provides end-user OS locale by default.
It can be overridden by the developer if needed.

The method `LoadCrowdinStrings` is async and can be awaited if needed.
<br>
In this example, the method is not “awaited” not to block the rendering of the user’s interface

---

### Configuration

The SDK provides developers two ways for resources loading configuration: global and per-request

```C#
var options = new CrowdinOptions
{
    DistributionHash = "<your_dist_hash>",
    NetworkPolicy = NetworkPolicy.OnlyWiFi,
    UseCache = true,
    FileName = "Translations.resx"
};
```

+ `NetworkPolicy` - for network restrictions
  + `All` - all network types allowed
  + `OnlyWiFi` or `OnlyCellular` - only needed type allowed
  + `Forbidden`

+ `UseCache` - turn on or off built-in caching mechanism

For global configuration override default GlobalOptions

```C#
DynamicResourcesLoader.GlobalOptions = options;
```

For per-request configuration pass `options` as the first parameter

```C#
DynamicResourcesLoader.LoadCrowdinStrings(options, Current.Resources);
```

In the last way don't forget to add `options.FileName` value