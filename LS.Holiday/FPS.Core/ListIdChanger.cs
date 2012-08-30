using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using DocumentFormat.OpenXml.Packaging;
using FPS.Diagnostics;

namespace FPS.Core
{
    public static class ListIdChanger
    {
        #region Fields
        private const string Marker = "xmlns:ns3='";
        private const int GuidLength = 36;
        private const string GuidPattern = "[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}";
        #endregion

        #region Methods
        /// <summary>
        /// Change list ID in document.
        /// </summary>
        /// <param name="stream">Document stream.</param>
        /// <param name="newListId">List ID of current list.</param>
        /// <returns>Returns document stream.</returns>
        public static Stream ChangeListId(Stream stream, string newListId)
        {
            try
            {
                using (WordprocessingDocument agreementDocument = WordprocessingDocument.Open(stream, true))
                {
                    var xmlAgreementDocument = new XmlDocument();
                    xmlAgreementDocument.Load(agreementDocument.MainDocumentPart.GetStream(FileMode.Open, FileAccess.ReadWrite));

                    var body = xmlAgreementDocument.LastChild.FirstChild;
                    string oldGuid = GetOldListId(body);
                    var matchChecker = new Regex(GuidPattern);

                    if (matchChecker.IsMatch(oldGuid))
                    {
                        body.InnerXml = body.InnerXml.Replace(oldGuid, newListId.ToUpper());
                        xmlAgreementDocument.Save(agreementDocument.MainDocumentPart.GetStream(FileMode.Open, FileAccess.ReadWrite));
                    }
                }

                return stream;
            }
            catch (Exception ex)
            {
                Logger.Instance.Log(ex, LogType.Error);
            }

            return Stream.Null;
        }

        /// <summary>
        /// Get list ID from document.
        /// </summary>
        /// <param name="body">Document body.</param>
        /// <returns>Returns document list ID.</returns>
        private static string GetOldListId(XmlNode body)
        {
            return body.InnerXml.Substring(body.InnerXml.IndexOf(Marker) + Marker.Length, GuidLength);
        }
        #endregion
    }
}
