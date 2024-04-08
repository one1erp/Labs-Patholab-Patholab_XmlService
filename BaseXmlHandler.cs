using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using LSSERVICEPROVIDERLib;
using MSXML;
using Patholab_Common;

namespace Patholab_XmlService
{
    public abstract class BaseXmlHandler
    {
        protected DOMDocument objDoc;
        protected DOMDocument objRes;
        private readonly INautilusProcessXML _processXml;
        private readonly string path;
        private bool _succes;
        private string _response;
        private string fileName = "";


        protected BaseXmlHandler(INautilusServiceProvider sp)
        {

            //Get xml processor
            _processXml = Utils.GetXmlProcessor(sp);
            //Get nautilus user
            var ntlsUser = Utils.GetNautilusUser(sp);
            //Get workstation of current user
            var workstationId = ntlsUser.GetWorkstationName();
            try
            {
                //זמני
                path = GetFullPath();


            }
            catch (Exception exception)
            {

                Logger.WriteLogFile(exception);
            }

        }
        protected BaseXmlHandler(INautilusServiceProvider sp, string fileName)
            : this(sp)
        {
            this.fileName = fileName;
        }
        /// <summary>
        /// Run xml
        /// </summary>
        /// <returns>Success</returns>        
        public bool ProcssXml()
        {
            objRes = new DOMDocument();
            _response = _processXml.ProcessXMLWithResponse(objDoc, objRes);
            _succes = _response.Length == 0;// CheckErrors(objRes);
            try
            {
                // שמירה של מסמך התשובה במקרה של שגיאה
                if (!_succes)
                {

                    string savePath = null;
                    var directoryPath = GetDirectoryPath();
                    if (directoryPath != null)
                    {
                        string dt = DateTime.UtcNow.ToString("yyyy-MM-dd HH-mm-ss-fff");

                        savePath = (directoryPath + "\\Res_" + fileName.MakeSafeFilename('_') + (dt));
                        objDoc.save(savePath + "doc_ERROR.xml");
                        objRes.save(savePath + "res_ERROR.xml");
                    }
                }



            }
            catch (Exception ex)
            {

                Logger.WriteLogFile(ex);
                // אם הוא לא מצליח לשמור 
            }

            return _succes;
        }

        public bool ProcssXmlWithOutResponse()
        {
            objRes = new DOMDocument();
            //_response = _processXml.ProcessXMLWithResponse(objDoc, objRes);
            _response = _processXml.ProcessXML(objDoc);

            try
            {
                var directoryPath = GetDirectoryPath();
                if (directoryPath != null)
                {
                    //     for test
                    //    objDoc.save(@"C:\temp\" + "Doc" + (DateTime.Now.ToString().MakeSafeFilename('_')) + ".xml");
                    //    objRes.save(@"C:\temp\" + "Res" + (DateTime.Now.ToString().MakeSafeFilename('_')) + ".xml");

                    string ut = DateTime.UtcNow.ToString("yyyy-MM-dd HH-mm-ss-fff");


                    objDoc.save(directoryPath + "\\Doc_" + fileName.MakeSafeFilename('_') + (ut) + ".xml");
                    objRes.save(directoryPath + "\\Res_" + fileName.MakeSafeFilename('_') + (ut) + ".xml");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLogFile(ex);
                // אם הוא לא מצליח לשמור 
            }

            _succes = _response.Length == 0;// CheckErrors(objRes);

            return _succes;
        }

        public bool ProcssXmlWithOutSave()
        {
            objRes = new DOMDocument();
            _response = _processXml.ProcessXML(objDoc);//, objRes);

            _succes = _response.Length == 0;// CheckErrors(objRes);

            return _succes;
        }
        /// <summary>
        /// Get directory to save docs
        /// </summary>
        /// <returns></returns>
        private string GetDirectoryPath()
        {



            if (!string.IsNullOrEmpty(path))
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }


            return path;
        }

        private bool CheckErrors(DOMDocument objRes)
        {
            try
            {
                IXMLDOMNode answer = objRes.getElementsByTagName("errors")[0];
                return answer == null;
            }
            catch (Exception ex)
            {
                Logger.WriteLogFile(ex);
            }
            return true;
        }

        /// <summary>
        /// Get response from xml by tag
        /// </summary>
        /// <param name="tagName">tag with response data</param>
        /// <returns></returns>
        public string GetValueByTagName(string tagName)
        {
            try
            {

                if (objRes != null)
                {
                    var newValue = ((dynamic)objRes.getElementsByTagName(tagName)[0]).nodeTypedValue;
                    return newValue != null ? newValue.ToString() : null;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLogFile(ex);

            }
            return null;

        }

        /// <summary>
        /// Get response from xml by tag
        /// </summary>
        /// <param name="tagName">tag with response data</param>
        /// <param name="index">Location of tag in response</param>
        /// <returns></returns>
        public object GetValueByTagName(string tagName, int index)
        {
            try
            {

                if (objRes != null)
                {
                    var newValue = ((dynamic)objRes.getElementsByTagName(tagName)[index]).nodeTypedValue;
                    return newValue != null ? newValue.ToString() : null;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLogFile(ex);

            }
            return null;
        }
        /// <summary>
        /// Get response from xml by tag
        /// </summary>
        /// <param name="tagName">tag with response data</param>
        /// <param name="index">Location of tag in response</param>
        /// <returns></returns>
        public object GetValueByTagName(string tagName, int index, int attributeIndex)
        {
            try
            {

                if (objRes != null)
                {
                    var newValue = ((dynamic)((objRes.getElementsByTagName(tagName)[index]))).attributes[attributeIndex].text;
                    return newValue != null ? newValue.ToString() : null;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLogFile(ex);

            }
            return null;
        }
        /// <summary>
        /// Get string error
        /// </summary>
        /// <returns></returns>
        public string ErrorResponse
        {
            get
            {
                if (!_succes)
                {
                    return _response;
                }
                return null;
            }
        }

        private static string GetFullPath()
        {


            try
            {


                string assemblyPath = Assembly.GetExecutingAssembly().Location;
                ExeConfigurationFileMap map = new ExeConfigurationFileMap();
                map.ExeConfigFilename = assemblyPath + ".config";
                Configuration cfg = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
                var appSettings = cfg.AppSettings;

                //Ashi 13/10/20 For citrix envirionment Create folder by user name
               // string path = Path.Combine(appSettings.Settings["XmlPath"].Value, Environment.MachineName);
                string path = Path.Combine(appSettings.Settings["XmlPath"].Value, Environment.UserName.MakeSafeFilename('_'));
                //string logFile = fileName;.. +"-" + DateTime.Now.ToString("dd-MM-yyyy");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
            catch (Exception e)
            {

                return null;
            }
        }
        protected string ConvertFindBy(FindBy findBy)
        {
            switch (findBy)
            {
                case FindBy.Id:
                    return "find-by-id";
                case FindBy.Name:
                    return "find-by-name";
                case FindBy.ExternalReference:
                    return "find-by-external-ref";
            }


            return "";
        }
    }
}
