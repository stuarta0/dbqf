using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DbQueryFramework_v1.Configuration
{
    /// <summary>
    /// Represents a view of the database to be used for user querying.
    /// </summary>
    public class QueryConfig
    {
        /// <summary>
        /// A unique identifier representing this configuration.
        /// </summary>
        [XmlAttribute]
        public Guid ConfigID { get; set; }
		
        /// <summary>
        /// Gets the list of subjects in this particular configuration.
        /// </summary>
        [XmlArray]
        public List<Subject> Subjects { get; set; }

        /// <summary>
        /// Gets the matrix of queries that allow this configuration to get 'from' subject A 'to' subject B.
        /// The dictionary is indexed by Subject.ID (from) followed by another index by Subject.ID (to) and the resulting
        /// value will be the query that relates these two subjects.  Query output MUST produce two columns: FromID and ToID.
        /// Uniqueness or order is irrelevant.
        /// </summary>
        [XmlElement]
        public SerializableDictionary<int, SerializableDictionary<int, MatrixNode>> SubjectMatrix { get; set; }

        public QueryConfig()
        {
            ConfigID = Guid.NewGuid();
            Subjects = new List<Subject>();
            SubjectMatrix = new SerializableDictionary<int, SerializableDictionary<int, MatrixNode>>();
        }


        public Subject GetSubject(int id)
        {
            return Subjects.Find((s) => { return s.ID == id; });
        }

        public Subject GetSubject(string displayName)
        {
            return Subjects.Find((s) => { return s.DisplayName.Equals(displayName); });
        }

    }
}
