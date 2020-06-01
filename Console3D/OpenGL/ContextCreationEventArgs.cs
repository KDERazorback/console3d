using System;

namespace Console3D.OpenGL
{
    public class ContextCreationEventArgs
    {
        public ContextCreationEventArgs(string api, Version ver, string strVer, string profile, string vendor)
        {
            DriverVersionString = strVer ?? throw new ArgumentNullException(nameof(strVer));
            Api = api ?? throw new ArgumentNullException(nameof(api));
            DriverVersion = ver ?? throw new ArgumentNullException(nameof(ver));
            DriverProfile = profile;
            DriverVendor = vendor;
        }

        public Version DriverVersion { get; }
        public string DriverVersionString { get; }
        public string Api { get; }

        public string DriverProfile { get; }
        public string DriverVendor { get; }

        public override string ToString()
        {
            return $"{Api} {DriverProfile} {DriverVersion} ({DriverVersionString} from {DriverVendor}";
        }
    }
}
