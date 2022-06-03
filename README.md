
[<p align='center'><img src='https://support.crowdin.com/assets/logos/crowdin-dark-symbol.png' data-canonical-src='https://support.crowdin.com/assets/logos/crowdin-dark-symbol.png' width='200' height='200' align='center'/></p>](https://crowdin.com)

# Crowdin Xamarin Forms SDK [<img src="https://img.shields.io/badge/beta-yellow"/>](https://github.com/crowdin/xamarin-sdk)

The Crowdin Xamarin Forms SDK delivers all new translations from Crowdin project to the application immediately. So there is no need to update the application via Store to get the new version with the localization.

[Example project](https://github.com/crowdin/xamarin-sdk/tree/main/examples/TestMobileApp) | [Crowdin docs](https://support.crowdin.com/content-delivery/) | [Crowdin Enterprise docs](https://support.crowdin.com/enterprise/content-delivery/)

[![Nuget](https://img.shields.io/nuget/v/Crowdin.Xamarin.Forms?cacheSeconds=5000&logo=nuget)](https://www.nuget.org/packages/Crowdin.Xamarin.Forms/)
[![Nuget](https://img.shields.io/nuget/dt/Crowdin.Xamarin.Forms?cacheSeconds=800&logo=nuget)](https://www.nuget.org/packages/Crowdin.Xamarin.Forms/)
![GitHub Workflow Status](https://img.shields.io/github/workflow/status/crowdin/xamarin-sdk/Build%20Library?logo=github)
[![GitHub issues](https://img.shields.io/github/issues/crowdin/xamarin-sdk?cacheSeconds=9000)](https://github.com/crowdin/xamarin-sdk/issues)
[![GitHub contributors](https://img.shields.io/github/contributors/crowdin/xamarin-sdk?cacheSeconds=9000)](https://github.com/crowdin/xamarin-sdk/graphs/contributors)
[![GitHub](https://img.shields.io/github/license/crowdin/xamarin-sdk?cacheSeconds=20000)](https://github.com/crowdin/xamarin-sdk/blob/master/LICENSE)

### Features

+ Load remote strings from Crowdin Over-The-Air Content Delivery Network
+ Built-in translations caching mechanism (enabled by default, can be disabled)
+ Network usage configuration (All, only Wi-Fi or Cellular, Forbidden)
+ Load static strings from the bundled RESX files (usable as fallback before the CDN strings loaded)


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

### Quick start

For applications using the XML resource localization files (RESX)

1) Add Crowdin Distribution Hash before any modules initialization:

    ```C#
    DynamicResourcesLoader.GlobalOptions.DistributionHash = "{your_distribution_hash}";
    ```

2) Load static strings from app resource files to use as fallback:

    ```C#
    DynamicResourcesLoader.LoadStaticStrings(Translations.ResourceManager, Current.Resources);
    ```

    The first argument is the source - `ResourceManager` of the generated class from resources group (`Translations.resx` and descendants).
    The second argument is the destination - `ResourceDictionary` where to place loaded strings:

    * Global: in `Application.Current.Resources`
    * Per-view: in `ContentPage.Resources`

3) Load strings from Crowdin Distributions CDN:

    ```C#
    string langCode = DynamicResourcesLoader.CurrentCulture.TwoLetterISOLanguageName;
    DynamicResourcesLoader.LoadCrowdinStrings($"Translations.{langCode}.resx", Current.Resources);
    ```

    The property `CurrentCulture` provides end-user OS locale by default.
    It can be overridden by the developer if needed.

    The method `LoadCrowdinStrings` is async and can be awaited if needed.

    In this example, the method is not “awaited” not to block the rendering of the user’s interface.

### Configuration

The SDK provides developers two ways for resources loading configuration: global and per-request:

```C#
var options = new CrowdinOptions
{
    DistributionHash = "<your_distribution_hash>",
    NetworkPolicy = NetworkPolicy.OnlyWiFi,
    UseCache = true,
    FileName = "Translations.resx"
};
```

+ `NetworkPolicy` - for network restrictions
  + `All` - all network types allowed
  + `OnlyWiFi` or `OnlyCellular` - only needed type allowed
  + `Forbidden`

+ `UseCache` - turn on or off built-in translations caching mechanism.

For global configuration override default `GlobalOptions`:

```C#
DynamicResourcesLoader.GlobalOptions = options;
```

For per-request configuration pass `options` as the first parameter:

```C#
DynamicResourcesLoader.LoadCrowdinStrings(options, Current.Resources);
```

In a last way don't forget to add `options.FileName` value. Please note - for this example we used the 'Translations.resx' file name. This name should correspond to the file name in Crowdin.

### Contribution

If you want to contribute please read the [Contributing](/CONTRIBUTING.md) guidelines.

### Seeking Assistance
If you find any problems or would like to suggest a feature, please feel free to file an issue on Github at [Issues Page](https://github.com/crowdin/xamarin-sdk/issues).

Need help working with Crowdin Xamarin Forms SDK or have any questions?
[Contact Customer Success Service](https://crowdin.com/contacts).

### License
<pre>
The Crowdin Xamarin Forms SDK is licensed under the MIT License.
See the LICENSE file distributed with this work for additional
information regarding copyright ownership.

Except as contained in the LICENSE file, the name(s) of the above copyright
holders shall not be used in advertising or otherwise to promote the sale,
use or other dealings in this Software without prior written authorization.
</pre>
