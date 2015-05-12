using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DbQueryFramework_v1.Actions
{
    /// <summary>
    /// Enables calling a protocol handler.
    /// </summary>
    [Serializable]
    public class UriAction : SubjectAction 
    {
        /// <summary>
        /// Gets or sets the URI address that will be navigated.
        /// </summary>
        [XmlElement]
        public string Address { get; set; }
        
    }
}
