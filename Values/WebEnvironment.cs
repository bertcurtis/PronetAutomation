using Common;

namespace PronetAutomation.Values
{
    public static class WebEnvironment
    {
        public const string Production = "www";
        public const string Staging = "alpha";
        public const string Dev = "dev-app";

        public static string AppBase()
        {
            string returnValue = BaseConfiguration.Base == Production ? "app" : "staging-app";
            if (BaseConfiguration.Base == Dev)
            {
                returnValue = Dev;
            }
            return returnValue;
        }
        public static string FullAppURL()
        {
            return "http://" + AppBase() + BaseConfiguration.Host;
        }
    }
}