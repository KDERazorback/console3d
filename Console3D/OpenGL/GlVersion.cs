using System;

namespace Console3D.OpenGL
{
    public class GlVersion
    {
        public GlVersion(string api, int major, int minor, int revision, string profile)
        {
            Api = api ?? throw new ArgumentNullException(nameof(api));
            Major = major;
            Minor = minor;
            Revision = revision;
            Profile = profile ?? throw new ArgumentNullException(nameof(profile));
        }

        public string Api { get; }
        public int Major { get; }
        public int Minor { get; }
        public int Revision { get; }
        public string Profile { get; }

        public Version Version => new Version(Major, Minor, 0, Revision);

        public static GlVersion Parse(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                return new GlVersion("", 0, 0, 0, "");

            string[] segments = data.Trim().Split(' ', 2);

            string[] verSegments = segments[0].Split('.');

            int major = int.Parse(verSegments[0].Trim());
            int minor = int.Parse(verSegments[1].Trim());
            int rev = int.Parse(verSegments[2].Trim());

            string profile = "OpenTK";
            string api = "Gl";

            return new GlVersion(api, major, minor, rev, profile);
        }
    }
}
