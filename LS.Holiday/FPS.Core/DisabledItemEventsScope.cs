using System;
using Microsoft.SharePoint;

namespace FPS.Core
{
    /// <summary>
    /// Place this object insinde a 'using' statement otherwise event firing will be disabled
    /// for the lifetime of the instance.
    /// </summary>
    public class DisabledItemEventsScope : SPItemEventReceiver, IDisposable
    {
        #region Fields

        private bool _oldValue;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DisabledItemEventsScope"/> class.
        /// </summary>
        public DisabledItemEventsScope()
        {
            _oldValue = base.EventFiringEnabled;
            base.EventFiringEnabled = false;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            base.EventFiringEnabled = _oldValue;
        }

        #endregion
    }
}