using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarterPack.Default
{
    public static class StringExtensions
    {
        public static String ToInvariant(this string str)
        {
            return str.ToLower().Replace(" ", "").Replace(".", "");
        }
    }
}
