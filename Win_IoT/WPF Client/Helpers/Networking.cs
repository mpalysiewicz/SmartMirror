using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Connectivity;

namespace ABB.MagicMirror.Helpers
{
    public class Networking
    {
        public static List<string> GetLocalIpAddress()
        {
            List<string> ipAddresses = new List<string>();
            foreach (HostName localHostName in NetworkInformation.GetHostNames())
            {
                if (localHostName.IPInformation != null)
                {
                    if (localHostName.Type == HostNameType.Ipv4)
                    {
                        ipAddresses.Add(localHostName.CanonicalName);
                    }
                }
            }

            return ipAddresses;
        }
    }
}
