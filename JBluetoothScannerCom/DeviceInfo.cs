using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComToPhone
{
    public class DeviceInfo
    {
        public String Name;
        public long Address;
        public DeviceInfo(String DeviceName, long DeviceAddress)
        {
            Name = DeviceName;
            Address = DeviceAddress;
        }
    }
}
