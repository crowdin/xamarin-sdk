using JetBrains.Annotations;

namespace Crowdin.Maui.Models;

[PublicAPI]
public enum NetworkPolicy
{
    All,
    OnlyWiFi,
    OnlyCellular,
    Forbidden
}