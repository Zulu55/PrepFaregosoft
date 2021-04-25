using Windows.Storage;

namespace Faregosoft.Helpers
{
    public class Settings
    {
        private static readonly ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;

        public static string GetApiUrl()
        {
            return (string)_localSettings.Values["ApiUrl"];
        }
    }
}
