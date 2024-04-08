using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSXML;
using LSSERVICEPROVIDERLib;

namespace Patholab_XmlService
{

    public class UpdateDynamicEntity : BaseXmlHandler
    {
        public UpdateDynamicEntity(INautilusServiceProvider sp)
            : base(sp)
        {
        }

        public UpdateDynamicEntity(INautilusServiceProvider sp, string fileName)
            : base(sp, fileName)
        {
        }

        private Dictionary<string, string> properties;
        private string _tableName;
        private FindBy _findBy;
        private string _value;



        public void LoginDynamic(string tableName, FindBy findBy, string value)
        {
            _tableName = tableName;

            _findBy = findBy;
            _value = value;
            objDoc = new DOMDocument();
            properties = new Dictionary<string, string>();

        }

        public void AddProperties(string field, string value)
        {

            properties.Add(field, value);
        }

        public bool ProcssXml()
        {
            // var request = BuildUpdateRequest(_tableName, _entityName, _findBy, _value);
            var q = BuildXml(_tableName, _findBy, _value);

            objDoc.loadXML(q);
            return base.ProcssXml();
        }




        private string BuildXml(string tableName,
         FindBy findByClause,//set enum
          string findByValue
        )
        {
            // Add lims-request node
            string request = "<login-request>";



            //    Add the table node
            request += String.Format("<{0}>", tableName);

            var findClause = this.ConvertFindBy(findByClause);

            request += String.Format("<{0}>{1}</{0}>", findClause, findByValue);

            foreach (var item in properties)
            {


                // Add the id clause
                request += String.Format("<{0}>{1}</{0}>", item.Key, item.Value);
            }
            //  Add the table node
            request += String.Format("</{0}>", tableName);



            //       Add lims-request node
            request += "</login-request>";

            //      Return the request
            return request;
        }











    }



}




