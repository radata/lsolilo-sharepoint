using System;

namespace LS.Delegations.Generated
{
    /// <summary>
    /// Contains list informations.
    /// </summary>
    public struct ListInformation
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ListInformation"/> struct.
        /// </summary>
        /// <param name="name">The name.</param>
        public ListInformation(string name) :
            this()
        {
            Name = name;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets list name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        #endregion
    }
}
