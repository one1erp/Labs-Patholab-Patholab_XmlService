using System;
using System.Collections.Generic;
using LSSERVICEPROVIDERLib;
using MSXML;

namespace Patholab_XmlService
{

    public class CreateStaticEntity : BaseXmlHandler
    {
        public CreateStaticEntity(INautilusServiceProvider sp)
            : base(sp)
        {
        }

        public CreateStaticEntity(INautilusServiceProvider sp, string fileName)
            : base(sp, fileName)
        {
        }

        Dictionary<string, string> properties;
        private string _tableName;
        private string _entityName;
        private string _name;
        public void Login(string tableName, string entityName, string name)
        {
            _tableName = tableName;
            _entityName = entityName;
            _name = name;
            objDoc = new DOMDocument();
            properties = new Dictionary<string, string>();

        }
        public void AddProperties(string field, string value)
        {

            properties.Add(field, value);
        }

        public bool ProcssXml()
        {
            var request = BuildCreateRequest(_tableName, _entityName, _name);


            objDoc.loadXML(request);
            return base.ProcssXml();
        }

        public string BuildCreateRequest(string tableName,
            string entityName,
            string name
       )
        {
            // Add lims-request node
            string request = "<lims-request>";

            // Add the lims-data-request node
            request += "<lims-data-request>";

            // Add the table node
            request += String.Format("<{0} EntityName=\"{1}\">", tableName, entityName);

            // Add the create clause
            request += String.Format("<create>");

            // Add the name node
            request += String.Format("<name>{0}</name>", name);

            // Check if we have to add a return node
            if ((properties != null) && (properties.Count > 0))
            {
                // Add the return node
                request += "<field-assignment>";

                // Add the fields
                foreach (var item in properties)
                {


                    // Add the id clause
                    request += String.Format("<{0}>{1}</{0}>", item.Key, item.Value);
                }

                // Close the return node
                request += "</field-assignment>";
            }

            // Close the create clause
            request += String.Format("</create>");

            // Add the table node
            request += String.Format("</{0}>", tableName);

            // Close the lims-data-request node 
            request += "</lims-data-request>";

            // Add lims-request node
            request += "</lims-request>";

            // Return the request
            return request;

        }
    }


}




//namespace OneToManyBlock
//{
//    public class BuildRequestObject
//    {
//        #region Public Static Methods

//        /// <summary>
//        /// This method will build a find-request using the parameters passed by the user
//        /// </summary>
//        /// <returns>The xml request</returns>
//        public static string BuildFindRequest(string tableName,
//            string findClause, 
//            string idClause, 
//            object[] idValues,
//            object[] fieldsReturned,
//            object[] childTables,
//            object[][] childTablesFields)
//        {
//            // Add lims-request node
//            string request = "<lims-request>";

//            // Add the find-request node
//            request += "<find-request>";

//            // Add the table node
//            request += String.Format("<{0}>", tableName);

//            // Add the find-by-clause
//            request += String.Format("<{0}>", findClause);

//            // Add the values to the find clause
//            foreach (object o in idValues)            
//            { 
//                // Add the id clause
//                request += String.Format("<{0}>{1}</{0}>", idClause, o.ToString());
//            }

//            // Close the find-by-clause
//            request += String.Format("</{0}>", findClause);

//            // Check if we have to add a return node
//            if ((fieldsReturned != null) && (fieldsReturned.Length > 0))
//                request = AddReturnFields(fieldsReturned, request);

//            // Check if we need to add child tables to the request
//            if ((childTables != null) && (childTables.Length > 0))
//            {
//                for( int i = 0; i<childTables.Length ; i++)
//                {
//                    request += String.Format("<{0}>", childTables[0].ToString());

//                    if ((childTablesFields != null) && (childTablesFields[i] != null) && (childTablesFields[i].Length > 0))
//                        request = AddReturnFields(childTablesFields[i], request);

//                    request += String.Format("</{0}>", childTables[0].ToString());
//                }
//            }

//            // Add the table node
//            request += String.Format("</{0}>", tableName);

//            // Close the find-request node 
//            request += "</find-request>";

//            // Add lims-request node
//            request += "</lims-request>";

//            // Return the request
//            return request;
//        }

//        /// <summary>
//        /// This method will build a Create request using the parameters passed by the user
//        /// </summary>
//        /// <returns>The xml request</returns>
//        public static string BuildCreateRequest(string tableName,
//            string entityName,
//            string name,
//            bool versioned,
//            string parentVersion,
//            object[] fieldList,
//            object[] fieldValues)
//        {
//            // Add lims-request node
//            string request = "<lims-request>";

//            // Add the lims-data-request node
//            request += "<lims-data-request>";

//            // Add the table node
//            request += String.Format("<{0} EntityName=\"{1}\">", tableName, entityName); 

//            // Add the create clause
//            // Check if we are using versioning
//            if( versioned)
//                request += String.Format("<create versioned=\"true\" parentversion=\"{0}\">", parentVersion);
//            else request += String.Format("<create>");

//            // Add the name node
//            request += String.Format("<name>{0}</name>", name);

//            // Check if we have to add a return node
//            if ((fieldList != null) && (fieldList.Length > 0))
//            {
//                // Add the return node
//                request += "<field-assignment>";

//                // Add the fields
//                for(int i = 0; i < fieldList.Length; i++)
//                {
//                    // Add the id clause
//                    request += String.Format("<{0}>{1}</{0}>", fieldList[i].ToString(),fieldValues[i].ToString());
//                }

//                // Close the return node
//                request += "</field-assignment>";
//            }

//            // Close the create clause
//            request += String.Format("</create>");

//            // Add the table node
//            request += String.Format("</{0}>", tableName);

//            // Close the lims-data-request node 
//            request += "</lims-data-request>";

//            // Add lims-request node
//            request += "</lims-request>";

//            // Return the request
//            return request;
//        }

//       c


//        /// <summary>
//        /// This method will build a Delete request using the parameters passed by the user
//        /// </summary>
//        /// <returns>The xml request</returns>
//        public static string BuildDeleteRequest(string tableName,
//            string entityName,
//            string findByClause,
//            string findByValue,
//            bool versioned,
//            string numVersion)

//        {
//            // Add lims-request node
//            string request = "<lims-request>";

//            // Add the lims-data-request node
//            request += "<lims-data-request>";

//            // Add the table node
//            request += String.Format("<{0} EntityName=\"{1}\">", tableName, entityName);

//            // Add the update clause
//            request += "<delete>";

//            // Add the find-by clause
//            if (versioned)
//                request += String.Format("<{0} NumVersion=\"{1}\">{2}</{0}>", findByClause, numVersion, findByValue);
//            else request += String.Format("<{0}>{1}</{0}>", findByClause, findByValue);

//            // Close the create clause
//            request += String.Format("</delete>");

//            // Add the table node
//            request += String.Format("</{0}>", tableName);

//            // Close the lims-data-request node 
//            request += "</lims-data-request>";

//            // Add lims-request node
//            request += "</lims-request>";

//            // Return the request
//            return request;
//        }

//        #endregion

//        #region Private Static Methods

//        private static string AddReturnFields(object[] fieldsReturned, string request)
//        {
//            // Add the return node
//            request += "<return>";

//            // Add the fields
//            foreach (object o in fieldsReturned)
//            {
//                // Add the id clause
//                request += String.Format("<{0}/>", o.ToString());
//            }

//            // Close the return node
//            request += "</return>";
//            return request;
//        }

//        #endregion
//    }
//}
