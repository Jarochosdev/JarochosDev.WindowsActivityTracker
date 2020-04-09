using JarochosDev.Utilities.Net.NetStandard.Common.Proxies;

namespace JarochosDev.WindowsActivityTracker.Common.Utilities
{
    public class EnvironmentUserNameExtractor : IUserNameExtractor
    {
        public string GetUserName()
        {
            return ProxyEnvironment.Instance().UserName;
        }
    }
}