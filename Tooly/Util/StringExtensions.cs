using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tooly.Util
{
    public static class StringExtensions
    {
        public static string Capitalize(this string instance)
            => instance[0].ToString().ToUpper() + instance.Substring(1);

        public static bool IsEmpty(this string instance)
            => string.IsNullOrEmpty(instance);

    }
}
