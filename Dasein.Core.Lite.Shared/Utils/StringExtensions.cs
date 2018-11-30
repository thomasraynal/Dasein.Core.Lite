using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public static class StringExtensions
    {
        public static String ToInvariant(this string str)
        {
            return str.ToLower().Replace(" ", "").Replace(".", "");
        }
    }
}
