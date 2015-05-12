using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DbQueryFramework_v1.Actions
{
    /// <summary>
    /// Enables the execution of a process.
    /// </summary>
    [Serializable]
    public class ProcessAction : SubjectAction 
    {
        /// <summary>
        /// Gets or sets the file path and name to start as a process.  This can include placeholders from fields in the Subject source SQL and also environment variables.
        /// </summary>
        [XmlElement]
        public string Filename { get; set; }

        /// <summary>
        /// Gets or sets the arguments to be used when opening the Filename.  This can include placeholders from fields in the Subject source SQL and also environment variables.
        /// </summary>
        [XmlElement]
        public string Arguments { get; set; }
    }
}
