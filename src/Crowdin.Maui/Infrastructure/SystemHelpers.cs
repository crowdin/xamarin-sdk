namespace Crowdin.Maui.Infrastructure;

internal static class SystemHelpers
{
    internal static bool IsNetworkConnected()
    {
        return Connectivity.NetworkAccess is NetworkAccess.Internet;
    }
}