using System;
using System.Collections.Generic;

namespace LS.Delegations.Generated
{
    /// <summary>
    /// Contains ContentType informations.
    /// </summary>
    /// <typeparam name="T">Fields class.</typeparam>
    public class ContentTypeInformation<T>
    {
        #region Properies
        /// <summary>
        /// Gets ContentType name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets ContentType ID.
        /// </summary>
        /// <value>
        /// The ID.
        /// </value>
        public string Id { get; private set; }

        /// <summary>
        /// Gets or sets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        public T Fields { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeInformation"/> struct.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        public ContentTypeInformation(string id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeInformation"/> struct.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        /// <param name="fields">The fields.</param>
        public ContentTypeInformation(string id, string name, T fields)
        {
            Id = id;
            Name = name;
            Fields = fields;
        }
        #endregion

        #region MyRegion
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }
        #endregion
    }
}
