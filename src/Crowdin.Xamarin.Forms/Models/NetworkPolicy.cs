
using JetBrains.Annotations;

namespace Crowdin.Xamarin.Forms.Models
{
    [PublicAPI]
    public enum NetworkPolicy
    {
        All,
        OnlyWiFi,
        OnlyCellular,
        Forbidden
    }
}