using System;
using System.Management;
using JarochosDev.Utilities.Net.NetStandard.Common.Proxies;
using JarochosDev.WindowsActivityTracker.Common.Utilities;

namespace JarochosDev.WindowsActivityTracker.WindowsService.Utilities
{
    public class WindowsServiceUserNameExtractor : IUserNameExtractor
    {
        public string GetUserName()
        {
            var username = ProxyEnvironment.Instance().UserName;
            if (!username.Equals("SYSTEM", StringComparison.CurrentCultureIgnoreCase))
            {
                return username;
            }

            try
            {
                // Define WMI scope to look for the Win32_ComputerSystem object
                ManagementScope ms = new ManagementScope("\\\\.\\root\\cimv2");
                ms.Connect();

                ObjectQuery query = new ObjectQuery
                    ("SELECT * FROM Win32_ComputerSystem");
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher(ms, query);

                // This loop will only run at most once.
                foreach (ManagementObject mo in searcher.Get())
                {
                    // Extract the username
                    username = mo["UserName"].ToString();
                }
                // Remove the domain part from the username
                string[] usernameParts = username.Split('\\');
                // The username is contained in the last string portion.
                username = usernameParts[usernameParts.Length - 1];
            }
            catch (Exception)
            {
                // The system currently has no users who are logged on
                // Set the username to "SYSTEM" to denote that
                username = "SYSTEM";
            }
            return username;
        }
    }
}