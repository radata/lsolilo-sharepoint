using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FPS.Core
{
    public class SafeParser
    {
        /// <summary>
        /// Converts the string representation of a number to its 32-bit signed integer.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <param name="isSuccess">If set to <c>true</c> converted successfully; otherwise <c>false</c>.</param>
        /// <returns>Converted value.</returns>
        public static int ToInt32(string value, out bool isSuccess)
        {
            int result = 0;
            isSuccess = int.TryParse(value, out result);
            return result;
        }

        /// <summary>
        /// Converts the string representation of a number to its 32-bit signed integer.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <returns>Converted value.</returns>
        public static int ToInt32(string value)
        {
            int result = 0;
            int.TryParse(value, out result);
            return result;
        }

        /// <summary>
        /// Converts the string representation of a number to its 32-bit signed integer.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <returns>Converted value.</returns>
        public static int? ToInt32Nullable(string value)
        {
            int result;
            return int.TryParse(value, out result) ? (int?)result : null;
        }
    }
}
