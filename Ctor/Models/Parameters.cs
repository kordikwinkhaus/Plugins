using System;
using System.Collections.Generic;

namespace Ctor.Models
{
    internal static class Parameters
    {
        internal static Dictionary<string, object> ForFrameType(int type, int color)
        {
            if (type <= 0) throw new ArgumentOutOfRangeException(nameof(type));
            if (color <= 0) throw new ArgumentOutOfRangeException(nameof(color));

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Type", type);
            parameters.Add("Color", color);
            return parameters;
        }

        internal static Dictionary<string, object> ForGlasspacket(string nrArt)
        {
            if (string.IsNullOrEmpty(nrArt)) throw new ArgumentNullException(nameof(nrArt));

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Article", nrArt);
            return parameters;
        }

        internal static Dictionary<string, object> ForFalseMullion(string nrArt, int color, bool isLeftSide)
        {
            if (string.IsNullOrEmpty(nrArt)) throw new ArgumentNullException(nameof(nrArt));
            if (color <= 0) throw new ArgumentOutOfRangeException(nameof(color));

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Article", nrArt);
            parameters.Add("Color", color);
            parameters.Add("Side", isLeftSide);
            return parameters;
        }
    }
}
