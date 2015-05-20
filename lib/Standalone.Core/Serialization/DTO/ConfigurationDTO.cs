using dbqf.Configuration;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Standalone.Core.Serialization.DTO
{
    [XmlRoot("Configuration")]
    [ProtoContract]
    public class ConfigurationDTO
    {
        /// <summary>
        /// Gets or sets an array of subjects for a configuration.
        /// </summary>
        [XmlArrayItem("Subject")]
        [ProtoMember(1)]
        public SubjectDTO[] Subjects { get; /* public setter for XML */ set; }

        /// <summary>
        /// Gets or sets a 2-dimensional lookup of subject indicies in the Subjects array that represents
        /// the matrix for a given configuration.  Since protobuf doesn't support nested/jagged arrays,
        /// we use a helper class to wrap the second tier.
        /// </summary>
        [XmlArrayItem("Node")]
        [ProtoMember(2)]
        public MatrixNodeDTO[] Matrix { get; /* public setter for XML */ set; }

        /// <summary>
        /// Gets or sets subject at index.
        /// </summary>
        public SubjectDTO this[int i]
        {
            get { return Subjects[i]; }
            set { Subjects[i] = value; }
        }

        /// <summary>
        /// Resolves 2-dimensional matrix lookup with single dimensional Matrix list.
        /// </summary>
        public MatrixNodeDTO this[int i, int j]
        {
            get { return Matrix[i * Subjects.Length + j]; }
            set { Matrix[i * Subjects.Length + j] = value; }
        }

        public ConfigurationDTO()
        {
        }

        public ConfigurationDTO(int size)
        {
            Subjects = new SubjectDTO[size];
            Matrix = new MatrixNodeDTO[size * size];
        }
    }
}
