using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPS.Core;
using FPS.Diagnostics;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace FPS.Core
{
    public class JobInstaller<T> where T : SPJobDefinition, new()
    {
        #region Methods

        /// <summary>
        /// Installs the specified job to web application.
        /// </summary>
        /// <param name="webApplication">The web application.</param>
        /// <param name="schedule">The schedule.</param>
        /// <param name="taregtWebApplicationName">Name of the taregt web application.</param>
        public static void Install(SPWebApplication webApplication, SPSchedule schedule, string taregtWebApplicationName)
        {
            var isInstalled = false;
            var errorMessage = string.Empty;
            var jobName = string.Empty;

            try
            {
                var job = Activator.CreateInstance(typeof(T), webApplication) as T;
                jobName = job.Title;

                if (webApplication.Name != taregtWebApplicationName)
                    return;

                UninstallJob(webApplication, jobName);

                job.Schedule = schedule;
                job.Update();
                isInstalled = true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
            }
            finally
            {
                var logType = LogType.Information;
                var logMessage = string.Format("Feature: '{0}' was{1} installed to '{2}'.", jobName, isInstalled ? string.Empty : "n't", webApplication.Name);

                if (!errorMessage.IsNullOrEmpty())
                {
                    logMessage = string.Format("{0}{1}Exception:{1}{2}", logMessage, Environment.NewLine, errorMessage);
                    logType = LogType.Error;
                }

                Logger.Instance.Log(logMessage, logType);
            }
        }

        /// <summary>
        /// Uninstalls the specified job from the web application.
        /// </summary>
        /// <param name="webApplication">The web application.</param>
        /// <param name="taregtWebApplicationName">Name of the taregt web application.</param>
        public static void Uninstall(SPWebApplication webApplication, string taregtWebApplicationName)
        {
            var errorMessage = string.Empty;
            var jobName = string.Empty;
            var isUninstalled = false;

            try
            {
                jobName = new T().Title;
                isUninstalled = UninstallJob(webApplication, jobName);
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
            }
            finally
            {
                var logType = LogType.Information;
                var logMessage = string.Format("Feature: '{0}' {1} '{2}'.", jobName, isUninstalled ? "was uninstalled from" : "wasn't found at", webApplication.Name);

                if (!errorMessage.IsNullOrEmpty())
                {
                    logType = LogType.Error;
                    logMessage = string.Format(
                        "Feature: '{0}' wasn't uninstalled from '{1}'.{2}Exception:{2}{3}",
                        jobName,
                        webApplication.Name,
                        Environment.NewLine,
                        errorMessage);
                }

                Logger.Instance.Log(logMessage, logType);
            }
        }

        private static bool UninstallJob(SPWebApplication webApplication, string jobName)
        {
            var isUninstalled = false;

            foreach (SPJobDefinition job in webApplication.JobDefinitions)
            {
                if (job.Name == jobName)
                {
                    job.Delete();
                    isUninstalled = true;
                }
            }

            return isUninstalled;
        }

        #endregion
    }
}
