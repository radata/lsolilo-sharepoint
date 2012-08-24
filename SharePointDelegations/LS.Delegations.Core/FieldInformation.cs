using System;

namespace LS.Delegations.Core
{
    /// <summary>
    /// Contains field informations.
    /// </summary>
    public struct FieldInformation
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldInformation"/> struct.
        /// </summary>
        /// <param name="guid">The giud.</param>
        /// <param name="name">The name.</param>
        /// <param name="displayName">The display name.</param>
        public FieldInformation(Guid guid, string name, string displayName)
            : this()
        {
            Guid = guid;
            Name = name;
            DisplayName = displayName;
        }

        #endregion

        #region Properies

        /// <summary>
        /// Gets field name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets field display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Gets field GUID.
        /// </summary>
        /// <value>
        /// The GUID.
        /// </value>
        public Guid Guid { get; private set; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="FieldGenerator.FieldInformation"/> to <see cref="System.Guid"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Guid(FieldInformation value)
        {
            return value.Guid;
        }

        #endregion
    }
}
