using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace FPS.Diagnostics.Features.Feature
{
    [Guid("81329cdb-9e68-4bb8-897a-1b3af34c54ff")]
    public class FeatureEventReceiver : SPFeatureReceiver
    {
        #region Fields

        private const string WebCofigKeyName = "ErrorHandlerModule";
        private const string WebCofigNode = "configuration/system.webServer/modules";
        private string assemblyValue = string.Empty;

        #endregion

        #region Methods

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            UpdateWebConfig(properties, true);
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            UpdateWebConfig(properties, false);
        }

        private void UpdateWebConfig(SPFeatureReceiverProperties properties, bool isInstallation)
        {
            try
            {
                assemblyValue = typeof(ErrorHandlerModule).AssemblyQualifiedName.ToString();

                SPWebApplication webApp = ((SPWeb)properties.Feature.Parent).Site.WebApplication;
                webApp.WebConfigModifications.Clear();

                SPWebConfigModification webConfigModifications = GetConfigKey(properties.Feature.DefinitionId.ToString());

                // Removing exising key
                if (webApp.WebConfigModifications.Contains(webConfigModifications))
                    webApp.WebConfigModifications.Remove(webConfigModifications);

                // Adding key
                if (isInstallation)
                    webApp.WebConfigModifications.Add(webConfigModifications);

                webApp.WebService.ApplyWebConfigModifications();
            }
            catch (Exception ex)
            {
                Logger.Instance.Log(ex, LogType.Error);
            }
        }

        private SPWebConfigModification GetConfigKey(string owner)
        {
            SPWebConfigModification webConfigModifications = new SPWebConfigModification();
            webConfigModifications = new SPWebConfigModification();
            webConfigModifications.Path = WebCofigNode;
            webConfigModifications.Name = string.Format("add[@name='{0}']", WebCofigKeyName);
            webConfigModifications.Sequence = 0;
            webConfigModifications.Owner = owner;

            webConfigModifications.Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode;
            webConfigModifications.Value = string.Format("<add name=\"{0}\" preCondition=\"integratedMode\" type=\"{1}\" />", WebCofigKeyName, assemblyValue);

            return webConfigModifications;
        }

        #endregion
    }
}
