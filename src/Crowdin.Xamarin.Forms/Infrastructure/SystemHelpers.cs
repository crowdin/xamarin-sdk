
using Xamarin.Essentials;

namespace Crowdin.Xamarin.Forms.Infrastructure
{
    internal static class SystemHelpers
    {
        internal static bool IsNetworkConnected()
        {
            return Connectivity.NetworkAccess is NetworkAccess.Internet;
        }
    }
}