using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FPS.Core
{
    public static class StringBuilderHelpers
    {
        #region Methods

        /// <summary>
        /// Determines whether this StringBuilder is null or its content is empty.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <returns>
        /// Is <c>true</c> if StringBuilder is null or its content is empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty(this StringBuilder stringBuilder)
        {
            return stringBuilder == null || stringBuilder.Length < 1;
        }

        #endregion
    }
}
