using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FPS.Diagnostics
{
    public class ErrorHandlerModule : IHttpModule
    {
        #region Fields

        private const string SourceFilter = "FPS";

        #endregion

        #region Methods

        public void Init(HttpApplication application)
        {
            application.Error += new EventHandler(ApplicationError);
        }

        protected void ApplicationError(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            Exception exception = context.Server.GetLastError();

            if (exception.InnerException != null && exception.InnerException.Source.Contains(SourceFilter))
                Logger.Instance.Log(exception.InnerException, LogType.Error);
        }

        public void Dispose()
        {
        }

        #endregion
    }
}
