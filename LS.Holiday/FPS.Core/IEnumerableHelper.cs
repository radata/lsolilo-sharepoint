using System;
using System.Collections.Generic;
using System.Linq;

namespace FPS.Core
{
    public static class IEnumerableHelper
    {
        /// <summary>
        /// Determines whether the specified collection is null or empty.
        /// </summary>
        /// <typeparam name="T">The type parameter.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>
        /// <c>True</c> if the specified collection is null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }

        /// <summary>
        /// Performs an action for each element of the collection.
        /// </summary>
        /// <typeparam name="T">The type parameter.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="action">The action.</param>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            if (collection.IsNullOrEmpty())
                return;

            foreach (T value in collection)
                action(value);
        }
    }
}
