using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Console3D.OpenGL
{
    public struct KhronosVersion
    {
		/// <summary>
		/// OpenGL ES 1.x API.
		/// </summary>
		public const string ApiGles1 = "gles1";

		/// <summary>
		/// OpenGL ES 2.x+ API.
		/// </summary>
		public const string ApiGles2 = "gles2";

		/// <summary>
		/// OpenGL API.
		/// </summary>
		public const string ApiGl = "gl";

        public KhronosVersion(int major, int minor, int revision, string api, string profile)
        {
            Major = major;
            Minor = minor;
            Revision = revision;
			Api = api;
			Profile = profile;
        }

		public KhronosVersion(int major, int minor, int revision, string api) : this(major, minor, revision, api, null) { }

        public int Major { get; }
        public int Minor { get; }
        public int Revision { get; }
        public string Api { get; }
        public string Profile { get; }

        public static KhronosVersion Parse(string input, string api)
        {
			if (input == null)
				throw new ArgumentNullException(nameof(input));

			// Determine version value (support up to 3 version numbers)
			Match versionMatch = Regex.Match(input, @"(?<Major>\d+)\.(?<Minor>\d+)(\.(?<Rev>\d+))?");
			if (versionMatch.Success == false)
				throw new ArgumentException($"unrecognized pattern '{input}'", nameof(input));

			int versionMajor = int.Parse(versionMatch.Groups["Major"].Value);
			int versionMinor = int.Parse(versionMatch.Groups["Minor"].Value);
			int versionRev = versionMatch.Groups["Rev"].Success ? int.Parse(versionMatch.Groups["Rev"].Value) : 0;

			if (versionMinor >= 10 && versionMinor % 10 == 0)
				versionMinor /= 10;

			if (Regex.IsMatch(input, "ES"))
			{
				switch (versionMajor)
				{
					case 1:
						api = ApiGles1;
						break;
					default:
						api = ApiGles2;
						break;
				}
			}
			else
			{
				if (api == null)
					api = ApiGl;
			}

			return new KhronosVersion(versionMajor, versionMinor, versionRev, api);
		}

    }
}
