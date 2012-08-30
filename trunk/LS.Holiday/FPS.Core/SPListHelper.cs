using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace FPS.Core
{
    public static class SPListHelper
    {
        /// <summary>
        /// Gets the item by id without throwing exception.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="id">The item id.</param>
        /// <returns>Returns list item or null.</returns>
        public static SPListItem GetItemByIdSafe(this SPList list, int id)
        {
            try
            {
                return list.GetItemById(id);
            }
            catch
            {
                // GetItemById throws exception if item doesn't exist.
                return null;
            }
        }
    }
}
