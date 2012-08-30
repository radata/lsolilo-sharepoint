using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPS.Diagnostics;
using Microsoft.SharePoint;

namespace FPS.Core
{
    public class ValidatorBase
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is valid.
        /// </summary>
        /// <value>
        /// <c>True</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        public string FieldName { get; set; }

        #endregion

        #region Constructors

        public ValidatorBase()
        {
            IsValid = true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validates the specified properties.
        /// </summary>
        /// <param name="properties">The properties.</param>
        public virtual void Validate(SPItemEventProperties properties)
        {
            IsValid = false;
            Message = "Validator not created properly.";
        }

        #endregion
    }
}
