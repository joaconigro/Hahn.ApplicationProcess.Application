using Hahn.Mobile.Properties;

namespace Hahn.Mobile.Helpers
{
    public static class ResourceHelper
    {
        public static string GetString(string key)
        {
            return Resources.ResourceManager.GetString(key);
        }
    }
}
