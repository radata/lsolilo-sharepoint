using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace FPS.Core
{
    public static class SPListItemCollectionHelper
    {
        /// <summary>
        /// Gets the folder by name.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="folderName">Name of the folder.</param>
        /// <returns>The folder.</returns>
        public static SPFolder GetFolderByName(this SPListItemCollection collection, string folderName)
        {
            foreach (SPListItem folder in collection)
            {
                if (folder.Folder.Exists && folder.Name == folderName)
                    return folder.Folder;
            }

            return null;
        }

        /// <summary>
        /// Checks if there are any items in collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns>True if there are items; otherwise false.</returns>
        public static bool Any(this SPListItemCollection collection)
        {
            return collection.Count > 0;
        }
    }
}
