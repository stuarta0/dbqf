using System.Xml.Serialization;

namespace dbqf.Serialization.DTO
{
    [XmlRoot("Configuration")]
    public class ConfigurationDTO
    {
        /// <summary>
        /// Gets or sets an array of subjects for a configuration.
        /// </summary>
        [XmlArrayItem("Subject")]
        public SubjectDTO[] Subjects { get; /* public setter for XML */ set; }

        /// <summary>
        /// Gets or sets subject at index.
        /// </summary>
        public SubjectDTO this[int i]
        {
            get { return Subjects[i]; }
            set { Subjects[i] = value; }
        }

        public ConfigurationDTO()
        {
        }

        public ConfigurationDTO(int size)
        {
            Subjects = new SubjectDTO[size];
        }
    }

    [XmlRoot("Configuration")]
    public class MatrixConfigurationDTO : ConfigurationDTO
    {
        /// <summary>
        /// Gets or sets a 2-dimensional lookup of subject indicies in the Subjects array that represents
        /// the matrix for a given configuration.  Since protobuf doesn't support nested/jagged arrays,
        /// we use a helper class to wrap the second tier.
        /// </summary>
        [XmlArrayItem("Node")]
        public MatrixNodeDTO[] Matrix { get; /* public setter for XML */ set; }

        /// <summary>
        /// Resolves 2-dimensional matrix lookup with single dimensional Matrix list.
        /// </summary>
        public MatrixNodeDTO this[int i, int j]
        {
            get { return Matrix[i * Subjects.Length + j]; }
            set { Matrix[i * Subjects.Length + j] = value; }
        }

        public MatrixConfigurationDTO()
            : base()
        {
        }

        public MatrixConfigurationDTO(int size)
            : base(size)
        {
            Matrix = new MatrixNodeDTO[size * size];
        }
    }
}
